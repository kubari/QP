using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.BLL.Helpers;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.BLL.Services.VisualEditor;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class VisualEditorRepository
    {
        internal static IEnumerable<VisualEditorPluginListItem> List(ListCommand cmd, int contentId, out int totalRecords)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorPluginsPage(scope.DbConnection, contentId, cmd.SortExpression, out totalRecords, cmd.StartRecord, cmd.PageSize);
            return QPContext.Map<VisualEditorPluginListItem[]>(rows.ToList());
        }

        /// <summary>
        /// Возвращает список Plugin по ids
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<VisualEditorPlugin> GetPluginList(IEnumerable<int> ids)
        {
            IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
            return QPContext.Map<VisualEditorPlugin[]>(
                QPContext.EFContext.VePluginSet
                    .Where(f => decIDs.Contains(f.Id))
                    .ToList()
                );
        }

        /// <summary>
        /// Возвращает список Command по ids
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<VisualEditorCommand> GetCommandList(IEnumerable<int> ids)
        {
            IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
            return QPContext.Map<VisualEditorCommand[]>(
                QPContext.EFContext.VeCommandSet
                    .Where(f => decIDs.Contains(f.Id))
                    .ToList()
                );
        }

        /// <summary>
        /// Возвращает список Style по ids
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<VisualEditorStyle> GetStyleList(IEnumerable<int> ids)
        {
            IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
            return QPContext.Map<VisualEditorStyle[]>(
                QPContext.EFContext.VeStyleSet
                    .Where(f => decIDs.Contains(f.Id))
                    .ToList());
        }

        internal static VisualEditorPlugin GetPluginPropertiesById(int id)
        {
            return QPContext.Map<VisualEditorPlugin>(
                QPContext.EFContext.VePluginSet
                .Include("LastModifiedByUser")
                .Include("VeCommands")
                .SingleOrDefault(g => g.Id == id)
            );
        }

        internal static VisualEditorPlugin UpdatePluginProperties(VisualEditorPlugin plugin)
        {
            var entities = QPContext.EFContext;
            DateTime timeStamp;
            using (var scope = new QPConnectionScope())
            {
                timeStamp = Common.GetSqlDate(scope.DbConnection);
            }

            var dal = QPContext.Map<VePluginDAL>(plugin);
            dal.LastModifiedBy = QPContext.CurrentUserId;
            dal.Modified = timeStamp;
            entities.Entry(dal).State = EntityState.Modified;
            UpdateCommands(plugin, entities, timeStamp);
            DefaultRepository.TurnIdentityInsertOn(EntityTypeCode.VisualEditorPlugin, plugin);
            entities.SaveChanges();
            DefaultRepository.TurnIdentityInsertOff(EntityTypeCode.VisualEditorPlugin);
            return GetPluginPropertiesById(plugin.Id);
        }

        private static void UpdateCommands(VisualEditorPlugin plugin, QPModelDataContext entities, DateTime timeStamp)
        {
            // delete
            var newIds = new HashSet<decimal>(plugin.VeCommands.Select(c => Converter.ToDecimal(c.Id)));
            var commandsToDelete = entities.VeCommandSet.Where(n => n.PluginId == plugin.Id && !newIds.Contains(n.Id));
            foreach (var cmd in commandsToDelete)
            {
                entities.Entry(cmd).State = EntityState.Deleted;
            }

            // save and update
            var forceIds = plugin.ForceCommandIds == null ? null : new Queue<int>(plugin.ForceCommandIds);
            foreach (var command in plugin.VeCommands)
            {
                var dalCommand = QPContext.Map<VeCommandDAL>(command);

                dalCommand.Modified = timeStamp;
                dalCommand.LastModifiedBy = QPContext.CurrentUserId;

                if (dalCommand.Id == 0)
                {
                    dalCommand.Created = timeStamp;
                    if (forceIds != null)
                    {
                        dalCommand.Id = forceIds.Dequeue();
                    }

                    entities.Entry(dalCommand).State = EntityState.Added;
                }
                else
                {
                    entities.Entry(dalCommand).State = EntityState.Modified;
                }
            }
        }

        internal static void Delete(int id)
        {
            DefaultRepository.Delete<VePluginDAL>(id);
        }

        internal static VisualEditorPlugin SavePluginProperties(VisualEditorPlugin plugin)
        {
            var entities = QPContext.EFContext;
            DateTime timeStamp;
            using (var scope = new QPConnectionScope())
            {
                timeStamp = Common.GetSqlDate(scope.DbConnection);
            }

            var dal = QPContext.Map<VePluginDAL>(plugin);
            dal.LastModifiedBy = QPContext.CurrentUserId;
            dal.Modified = timeStamp;
            dal.Created = timeStamp;

            entities.Entry(dal).State = EntityState.Added;

            DefaultRepository.TurnIdentityInsertOn(EntityTypeCode.VisualEditorPlugin, plugin);
            if (plugin.ForceId != 0)
            {
                dal.Id = plugin.ForceId;
            }

            entities.SaveChanges();
            DefaultRepository.TurnIdentityInsertOff(EntityTypeCode.VisualEditorPlugin);

            var forceIds = plugin.ForceCommandIds == null ? null : new Queue<int>(plugin.ForceCommandIds);
            foreach (var command in plugin.VeCommands)
            {
                var dalCommand = QPContext.Map<VeCommandDAL>(command);
                dalCommand.PluginId = dal.Id;
                if (forceIds != null)
                {
                    dalCommand.Id = forceIds.Dequeue();
                }

                dalCommand.LastModifiedBy = QPContext.CurrentUserId;
                dalCommand.Modified = timeStamp;
                dalCommand.Created = timeStamp;
                entities.Entry(dalCommand).State = EntityState.Added;

            }

            DefaultRepository.TurnIdentityInsertOn(EntityTypeCode.VisualEditorCommand);
            entities.SaveChanges();
            DefaultRepository.TurnIdentityInsertOff(EntityTypeCode.VisualEditorCommand);

            return QPContext.Map<VisualEditorPlugin>(dal);
        }

        internal static int GetCommandMaxRowOrder()
        {
            using var scope = new QPConnectionScope();
            return Common.GetVeCommandsMaxRowOrder(scope.DbConnection);
        }

        internal static int GetPluginMaxOrder()
        {
            using var scope = new QPConnectionScope();
            return Common.GetVisualEditorPluginMaxOrder(scope.DbConnection);
        }

        internal static int GetStyleMaxOrder()
        {
            using var scope = new QPConnectionScope();
            return Common.GetVisualEditorStyleMaxOrder(scope.DbConnection);
        }

        internal static IEnumerable<VisualEditorCommand> GetDefaultCommands()
        {
            return GetAllCommands()
                .OrderBy(c => c.RowOrder)
                .ThenBy(c => c.ToolbarInRowOrder)
                .ThenBy(c => c.GroupInToolbarOrder)
                .ThenBy(c => c.CommandInGroupOrder);
        }

        private static IEnumerable<VisualEditorCommand> GetAllCommands()
        {
            return QPContext.Map<VisualEditorCommand[]>(QPContext.EFContext.VeCommandSet.ToList());
        }

        internal static IEnumerable<VisualEditorCommand> GetSiteCommands(int siteId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorCommandsBySiteId(scope.DbConnection, siteId).ToList();
            return QPContext.Map<VisualEditorCommand[]>(rows);
        }

        private static IEnumerable<VisualEditorCommand> GetFieldCommands(int fieldId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorCommandsByFieldId(scope.DbConnection, fieldId).ToList();
            return QPContext.Map<VisualEditorCommand[]>(rows);
        }

        internal static IEnumerable<VisualEditorCommand> GetResultCommands(int fieldId, int siteId)
        {
            var defaultCommands = GetDefaultCommands();
            var siteCommands = GetSiteCommands(siteId);
            var fieldCommands = GetFieldCommands(fieldId);
            return VisualEditorHelpers.Merge(VisualEditorHelpers.Merge(defaultCommands, siteCommands), fieldCommands);
        }

        internal static IEnumerable<VisualEditorCommand> GetResultCommands(int siteId)
        {
            var defaultCommands = GetDefaultCommands();
            var siteCommands = GetSiteCommands(siteId);
            return VisualEditorHelpers.Merge(defaultCommands, siteCommands);
        }

        internal static void SetSiteCommands(int siteId, Dictionary<int, bool> changedCommands, Dictionary<int, bool> defaultCommandsDictionary)
        {
            using var scope = new QPConnectionScope();
            foreach (var rec in changedCommands)
            {
                if (defaultCommandsDictionary.ContainsKey(rec.Key) && defaultCommandsDictionary[rec.Key] == rec.Value)
                {
                    // совпадает с дефолтными настройками
                    Common.RemoveSiteVeCommand(scope.DbConnection, siteId, rec.Key);
                }
                else
                {
                    Common.UpdateOrInsertSiteVeCommandValue(scope.DbConnection, siteId, rec.Key, rec.Value);
                }
            }
        }

        internal static void SetFieldCommands(int siteId, int fieldId, Dictionary<int, bool> changedCommands, Dictionary<int, bool> defaultCommandsDictionary,
            Dictionary<int, bool> siteCommandsDictionary)
        {
            using var scope = new QPConnectionScope();
            foreach (var rec in changedCommands)
            {
                if (siteCommandsDictionary.TryGetValue(rec.Key, out var value))
                {
                    // совпадает с настройками сайта
                    if (value == rec.Value)
                    {
                        Common.RemoveFieldVeCommand(scope.DbConnection, fieldId, rec.Key);
                    }
                    else
                    {
                        Common.UpdateOrInsertFieldVeCommandValue(scope.DbConnection, fieldId, rec.Key, rec.Value);
                    }
                }
                else
                {
                    if (defaultCommandsDictionary.TryGetValue(rec.Key, value: out var value1))
                    {
                        // совпадает с дефолтными настройками
                        if (value1 == rec.Value)
                        {
                            Common.RemoveFieldVeCommand(scope.DbConnection, fieldId, rec.Key);
                        }
                        else
                        {
                            Common.UpdateOrInsertFieldVeCommandValue(scope.DbConnection, fieldId, rec.Key, rec.Value);
                        }
                    }
                }
            }
        }

        internal static bool IsCommandNameFree(string name, int pluginId)
        {
            using var scope = new QPConnectionScope();
            return Common.IsVeCommandNameFree(scope.DbConnection, name, pluginId);
        }

        internal static bool IsCommandAliasFree(string alias, int pluginId)
        {
            using var scope = new QPConnectionScope();
            return Common.IsVeCommandAliasFree(scope.DbConnection, alias, pluginId);
        }

        internal static IEnumerable<VisualEditorStyleListItem> ListStyles(ListCommand cmd, int contentId, out int totalRecords)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorStylesPage(scope.DbConnection, contentId, cmd.SortExpression, out totalRecords, cmd.StartRecord, cmd.PageSize);
            return QPContext.Map<VisualEditorStyleListItem[]>(rows.ToList());
        }

        internal static VisualEditorStyle GetStylePropertiesById(int id)
        {
            return QPContext.Map<VisualEditorStyle>(QPContext.EFContext.VeStyleSet
                .Include("LastModifiedByUser")
                .SingleOrDefault(g => g.Id == id)
            );
        }

        internal static VisualEditorStyle UpdateStyleProperties(VisualEditorStyle visualEditorStyle) => DefaultRepository.Update<VisualEditorStyle, VeStyleDAL>(visualEditorStyle);

        internal static void DeleteStyle(int id)
        {
            DefaultRepository.Delete<VeStyleDAL>(id);
        }

        internal static VisualEditorStyle SaveStyleProperties(VisualEditorStyle visualEditorStyle)
        {
            DefaultRepository.TurnIdentityInsertOn(EntityTypeCode.VisualEditorStyle, visualEditorStyle);
            var result = DefaultRepository.Save<VisualEditorStyle, VeStyleDAL>(visualEditorStyle);
            DefaultRepository.TurnIdentityInsertOff(EntityTypeCode.VisualEditorStyle);
            return result;
        }

        internal static IEnumerable<VisualEditorStyle> GetAllStyles()
        {
            var styles = QPContext.EFContext.VeStyleSet.OrderBy(n => n.Order).ToList();
            return QPContext.Map<VisualEditorStyle[]>(styles);
        }

        internal static IEnumerable<VisualEditorStyle> GetSiteStyles(int siteId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorStylesBySiteId(scope.DbConnection, siteId).ToList();
            return QPContext.Map<VisualEditorStyle[]>(rows);
        }

        private static IEnumerable<VisualEditorStyle> GetFieldStyles(int fieldId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetVisualEditorStylesByFieldId(scope.DbConnection, fieldId).ToList();
            return QPContext.Map<VisualEditorStyle[]>(rows);
        }

        internal static IEnumerable<VisualEditorStyle> GetResultStyles(int siteId)
        {
            var defaultStyles = GetAllStyles();
            var siteStyles = GetSiteStyles(siteId);
            return VisualEditorHelpers.Merge(defaultStyles, siteStyles);
        }

        internal static IEnumerable<VisualEditorStyle> GetResultStyles(int fieldId, int siteId)
        {
            var defaultStyles = GetAllStyles();
            var siteStyles = GetSiteStyles(siteId);
            var fieldStyles = GetFieldStyles(fieldId);
            return VisualEditorHelpers.Merge(VisualEditorHelpers.Merge(defaultStyles, siteStyles), fieldStyles);
        }

        internal static void SetSiteStyles(int siteId, Dictionary<int, bool> changedStyles, Dictionary<int, bool> defaultStyleDictionary)
        {
            using var scope = new QPConnectionScope();
            foreach (var rec in changedStyles)
            {
                if (defaultStyleDictionary.ContainsKey(rec.Key) && defaultStyleDictionary[rec.Key] == rec.Value)
                {
                    // совпадает с дефолтными настройками
                    Common.RemoveSiteVeStyle(scope.DbConnection, siteId, rec.Key);
                }
                else
                {
                    Common.UpdateOrInsertSiteVeStyleValue(scope.DbConnection, siteId, rec.Key, rec.Value);
                }
            }
        }

        internal static void SetFieldStyles(int siteId, int fieldId, Dictionary<int, bool> changedStyles, Dictionary<int, bool> defaultStylesDictionary, Dictionary<int, bool> siteStylesDictionary)
        {
            using var scope = new QPConnectionScope();
            foreach (var rec in changedStyles)
            {
                if (siteStylesDictionary.TryGetValue(rec.Key, out var value1))
                {
                    // совпадает с настройками сайта
                    if (value1 == rec.Value)
                    {
                        Common.RemoveFieldVeStyle(scope.DbConnection, fieldId, rec.Key);
                    }
                    else
                    {
                        Common.UpdateOrInsertFieldVeStyleValue(scope.DbConnection, fieldId, rec.Key, rec.Value);
                    }
                }
                else
                {
                    if (defaultStylesDictionary.TryGetValue(rec.Key, value: out var value))
                    {
                        // совпадает с дефолтными настройками
                        if (value == rec.Value)
                        {
                            Common.RemoveFieldVeStyle(scope.DbConnection, fieldId, rec.Key);
                        }
                        else
                        {
                            Common.UpdateOrInsertFieldVeStyleValue(scope.DbConnection, fieldId, rec.Key, rec.Value);
                        }
                    }
                }
            }
        }

        internal static Dictionary<int, bool> GetCommandBindingBySiteId(int siteId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetCommandBindingBySiteId(scope.DbConnection, siteId)
                .ToDictionary(r => Converter.ToInt32(r.Field<decimal>("COMMAND_ID")), r => r.Field<bool>("ON"));
        }

        internal static Dictionary<int, bool> GetCommandBindingByFieldId(int fieldId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetCommandBindingByFieldId(scope.DbConnection, fieldId)
                .ToDictionary(r => Converter.ToInt32(r.Field<decimal>("COMMAND_ID")), r => r.Field<bool>("ON"));
        }

        internal static Dictionary<int, bool> GetStyleBindingByFieldId(int fieldId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetStyleBindingByFieldId(scope.DbConnection, fieldId).ToDictionary(r => Converter.ToInt32(r.Field<decimal>("STYLE_ID")), r => r.Field<bool>("ON"));
        }

        internal static Dictionary<int, bool> GetStyleBindingBySiteId(int siteId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetStyleBindingBySiteId(scope.DbConnection, siteId).ToDictionary(r => Converter.ToInt32(r.Field<decimal>("STYLE_ID")), r => r.Field<bool>("ON"));
        }

        internal static Dictionary<int, bool> GetCommandMappingByFieldId(int fieldId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetCommandBindingByFieldId(scope.DbConnection, fieldId).ToDictionary(r => Converter.ToInt32(r.Field<decimal>("COMMAND_ID")), r => r.Field<bool>("ON"));
        }
    }
}
