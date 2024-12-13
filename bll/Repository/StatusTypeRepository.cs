using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class StatusTypeRepository
    {
        internal static StatusType GetById(int id)
        {
            var result = GetByIdFromCache(id);
            return result ?? GetRealById(id);
        }

        private static StatusType GetRealById(int id)
        {
            return QPContext.Map<StatusType>(
                QPContext.EFContext.StatusTypeSet.Include("LastModifiedByUser").SingleOrDefault(n => (int)n.Id == id)
                );
        }

        private static StatusType GetByIdFromCache(int id)
        {
            StatusType result = null;
            var cache = QPContext.GetStatusTypeCache();
            if (cache != null && cache.TryGetValue(id, out var value))
            {
                result = value;
            }
            return result;
        }

        internal static StatusType GetByName(string name, int siteId)
        {
            return QPContext.Map<StatusType>(
                QPContext.EFContext.StatusTypeSet.SingleOrDefault(n => n.Name == name && (int)n.SiteId == siteId)
                );
        }

        internal static IEnumerable<StatusType> GetStatusList(int siteId)
        {
            return QPContext.Map<StatusType[]>(
                QPContext.EFContext.StatusTypeSet.Where(s => (int)s.SiteId == siteId).OrderBy(n => n.Weight).ToList()
                );
        }

        internal static IEnumerable<StatusType> GetAll() => QPContext.Map<StatusType[]>(QPContext.EFContext.StatusTypeSet.ToList());

        internal static IEnumerable<StatusType> GetColouredStatuses()
        {
            return QPContext.Map<StatusType[]>(QPContext.EFContext.StatusTypeSet.Where(s => s.Color != null && s.AltColor != null).ToList());
        }

        /// <summary>
        /// Возвращает список по ids
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<StatusType> GetList(IEnumerable<int> ids)
        {
            var result = new List<StatusType>();
            var cache = QPContext.GetStatusTypeCache();
            if (cache != null)
            {
                foreach (var id in ids)
                {
                    result.Add(cache.TryGetValue(id, out var value) ? value : GetRealById(id));
                }
            }
            else
            {
                IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
                result = QPContext.Map<List<StatusType>>(
                    QPContext.EFContext.StatusTypeSet
                        .Where(f => decIDs.Contains(f.Id))
                        .ToList()
                    );
            }

            return result;
        }

        internal static IEnumerable<StatusTypeListItem> GetStatusTypePage(ListCommand cmd, int siteId, out int totalRecords)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetStatusTypePage(scope.DbConnection, siteId, "WEIGHT", out totalRecords, cmd.StartRecord, cmd.PageSize);
            return QPContext.Map<StatusTypeListItem[]>(rows.ToList());
        }

        internal static StatusType UpdateProperties(StatusType statusType) => DefaultRepository.Update<StatusType, StatusTypeDAL>(statusType);

        internal static StatusType SaveProperties(StatusType statusType)
        {
            DefaultRepository.TurnIdentityInsertOn(EntityTypeCode.StatusType);
            var result = DefaultRepository.Save<StatusType, StatusTypeDAL>(statusType);
            DefaultRepository.TurnIdentityInsertOff(EntityTypeCode.StatusType);
            return result;
        }

        internal static IEnumerable<int> GetWeightsBySiteId(int siteId, int exceptId = 0)
        {
            using var scope = new QPConnectionScope();
            var result = new List<int>();
            var rows = Common.GetStatusTypeWeightsBySiteId(scope.DbConnection, siteId, exceptId);
            foreach (var row in rows)
            {
                result.Add(Converter.ToInt32(row.Field<decimal>("WEIGHT")));
            }

            return result;
        }

        internal static void Delete(int id)
        {
            DefaultRepository.Delete<StatusTypeDAL>(id);
        }

        internal static bool IsInUseWithArticle(int id)
        {
            using var scope = new QPConnectionScope();
            return Common.GetNumberOfArticlesUsingStatusByStatusId(scope.DbConnection, id) != 0;
        }

        internal static bool IsInUseWithWorkflow(int id)
        {
            using var scope = new QPConnectionScope();
            return Common.GetNumberOfWorkflowsUsingStatusByStatusId(scope.DbConnection, id) != 0;
        }

        internal static void SetNullAssociatedNotificationsStatusTypesIds(int id)
        {
            using var scope = new QPConnectionScope();
            Common.SetNullAssociatedNotificationsStatusTypesIds(scope.DbConnection, id);
        }

        internal static void RemoveAssociatedContentItemsStatusHistoryRecords(int id)
        {
            using var scope = new QPConnectionScope();
            Common.RemoveAssociatedContentItemsStatusHistoryRecords(scope.DbConnection, id);
        }

        internal static void RemoveAssociatedWaitingForApprovalRecords(int id)
        {
            using var scope = new QPConnectionScope();
            Common.RemoveAssociatedWaitingForApprovalRecords(scope.DbConnection, id);
        }

        internal static ListResult<StatusTypeListItem> GetPageForWorkflow(ListCommand listCommand, int[] selectedIds, int workflowId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetStatusPageForWorkflow(scope.DbConnection, workflowId, listCommand.SortExpression, out var totalRecords, listCommand.StartRecord, listCommand.PageSize);
            return new ListResult<StatusTypeListItem> { Data = QPContext.Map<List<StatusTypeListItem>>(rows.ToList()), TotalRecords = totalRecords };
        }

        internal static List<StatusTypeListItem> GetAllForWorkflow(int workflowId)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetAllStatusesForWorkflow(scope.DbConnection, workflowId);
            return QPContext.Map<List<StatusTypeListItem>>(rows.ToList());
        }

        internal static IEnumerable<ListItem> GetStatusSimpleList(int[] selectedEntitiesIDs)
        {
            var decStatusIDs = Converter.ToDecimalCollection(selectedEntitiesIDs);
            return QPContext.EFContext.StatusTypeSet
                    .Where(n => decStatusIDs.Contains(n.Id))
                    .Select(g => new { g.Id, g.Name })
                    .ToArray()
                    .Select(g => new ListItem { Value = g.Id.ToString(CultureInfo.InvariantCulture), Text = g.Name })
                ;
        }

        internal static int GetPublishedStatusIdBySiteId(int siteId)
        {
            return (int)QPContext.EFContext.StatusTypeSet.Single(x => x.SiteId == siteId && x.Name == StatusName.Published).Id;
        }

        internal static int GetNoneStatusIdBySiteId(int siteId)
        {
            return (int)QPContext.EFContext.StatusTypeSet.Single(x => x.SiteId == siteId && x.Name == StatusName.None).Id;
        }
    }
}
