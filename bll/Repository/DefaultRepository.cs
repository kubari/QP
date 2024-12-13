using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class DefaultRepository
    {
        internal static TBiz Save<TBiz, TDal>(TBiz item)
            where TDal : class, IQpEntityObject
            where TBiz : EntityObject => SaveAsUser<TBiz, TDal>(item, QPContext.CurrentUserId);

        internal static TBiz SaveAsAdmin<TBiz, TDal>(TBiz item)
            where TDal : class, IQpEntityObject
            where TBiz : EntityObject => SaveAsUser<TBiz, TDal>(item, SpecialIds.AdminUserId);

        private static TBiz SaveAsUser<TBiz, TDal>(TBiz item, int userId)
            where TDal : class, IQpEntityObject
            where TBiz : EntityObject
        {
            var entities = QPContext.EFContext;
            var dalItem = QPContext.Map<TDal>(item);

            if (item.ForceId != 0)
            {
                dalItem.Id = item.ForceId;
            }

            DateTime current;
            using (new QPConnectionScope())
            {
                current = Common.GetSqlDate(QPConnectionScope.Current.DbConnection);
            }

            dalItem.Created = current;
            dalItem.Modified = current;
            dalItem.LastModifiedBy = userId;
            entities.Entry(dalItem).State = EntityState.Added;
            entities.SaveChanges();

            return QPContext.Map<TBiz>(dalItem);
        }

        internal static TBiz Update<TBiz, TDal>(TBiz item)
            where TDal : class, IQpEntityObject
            where TBiz : EntityObject
        {
            var dalItem = QPContext.Map<TDal>(item);
            var entities = QPContext.EFContext;
            dalItem.LastModifiedBy = QPContext.CurrentUserId;

            using (new QPConnectionScope())
            {
                dalItem.Modified = Common.GetSqlDate(QPConnectionScope.Current.DbConnection);
            }

            entities.Entry(dalItem).State = EntityState.Modified;
            entities.SaveChanges();
            return QPContext.Map<TBiz>(dalItem);
        }

        internal static void Delete<TDal>(int id)
            where TDal : class
        {
            var entities = QPContext.EFContext;
            var result = entities.Set<TDal>().Find((decimal)id);
            if (result != null)
            {
                entities.Entry( result).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        internal static void Delete<TDal>(int[] id)
            where TDal : class, IQpEntityObject
        {
            var entities = QPContext.EFContext;

            var list = FindAll<TDal>(entities, id.Select(n => (decimal)n).ToArray());
            foreach (var item in list)
            {
                entities.Entry(item).State = EntityState.Deleted;
            }

            entities.SaveChanges();
        }


        private static List<TDal> FindAll<TDal>(DbContext dbContext, decimal[] keyValues)
            where TDal : class, IQpEntityObject
        {
            return dbContext.Set<TDal>().Where(x => keyValues.Contains(x.Id)).ToList();
        }




        internal static TDal GetById<TDal>(int id, QPModelDataContext context = null)
            where TDal : class
        {
            var currentContext = context ?? QPContext.EFContext;
            return currentContext.Set<TDal>().Find((decimal)id);

        }

        internal static TDal SimpleSave<TDal>(TDal dalItem)
            where TDal : class
        {
            var entities = QPContext.EFContext;
            entities.Entry(dalItem).State = EntityState.Added;
            entities.SaveChanges();
            return dalItem;
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        internal static IEnumerable<TDal> SimpleSaveBulk<TDal>(IEnumerable<TDal> dalItemList)
            where TDal : class
        {
            var entities = QPContext.EFContext;
            foreach (var item in dalItemList)
            {
                entities.Entry(item).State = EntityState.Added;
            }
            entities.SaveChanges();
            return dalItemList;
        }

        internal static TDal SimpleUpdate<TDal>(TDal dalItem)
            where TDal : class
        {
            var entities = QPContext.EFContext;
            entities.Entry(dalItem).State = EntityState.Modified;
            entities.SaveChanges();
            return dalItem;
        }

        internal static void SimpleDelete<TDal>(TDal dalItem, QPModelDataContext context = null)
            where TDal : class
        {
            var entities = context ?? QPContext.EFContext;
            entities.Entry(dalItem).State = EntityState.Deleted;
            entities.SaveChanges();
        }

        internal static void SimpleDeleteBulk<TDal>(IEnumerable<TDal> dalItems, QPModelDataContext context = null)
            where TDal : class
        {
            var entities = context ?? QPContext.EFContext;
            foreach (var dal in dalItems)
            {
                entities.Entry(dal).State = EntityState.Deleted;
            }
            entities.SaveChanges();

        }

        private static void ChangeIdentityInsertState(string entityTypeCode, bool state)
        {
            if (QPContext.DatabaseType != DatabaseType.SqlServer)
            {
                return;
            }

            if (QPConnectionScope.Current.IdentityInsertOptions.Contains(entityTypeCode))
            {
                var table = TableToInsert(entityTypeCode);

                if (table != null)
                {
                    Common.ChangeInsertIdentityState(QPConnectionScope.Current.DbConnection, "dbo." + table, state);
                }
            }
        }

        private static string TableIdToInsert(string entityTypeCode)
        {
            string tableId;
            switch (entityTypeCode)
            {
                case EntityTypeCode.ContentLink:
                    tableId = "link_id";
                    break;
                case EntityTypeCode.Content:
                    tableId = "content_id";
                    break;
                case EntityTypeCode.ContentGroup:
                    tableId = "content_group_id";
                    break;
                case EntityTypeCode.Field:
                    tableId = "attribute_id";
                    break;
                case EntityTypeCode.Site:
                    tableId = "site_id";
                    break;
                case EntityTypeCode.StatusType:
                    tableId = "status_type_id";
                    break;
                case EntityTypeCode.Workflow:
                    tableId = "workflow_id";
                    break;
                case EntityTypeCode.Notification:
                    tableId = "notification_id";
                    break;
                default:
                    tableId = "id";
                    break;
            }

            return tableId;
        }

        private static string TableToInsert(string entityTypeCode)
        {
            string table = null;
            switch (entityTypeCode)
            {
                case EntityTypeCode.ContentLink:
                    table = "CONTENT_TO_CONTENT";
                    break;
                case EntityTypeCode.Content:
                    table = "CONTENT";
                    break;
                case EntityTypeCode.ContentGroup:
                    table = "CONTENT_GROUP";
                    break;
                case EntityTypeCode.Field:
                    table = "CONTENT_ATTRIBUTE";
                    break;
                case EntityTypeCode.VisualEditorCommand:
                    table = "VE_COMMAND";
                    break;
                case EntityTypeCode.VisualEditorPlugin:
                    table = "VE_PLUGIN";
                    break;
                case EntityTypeCode.VisualEditorStyle:
                    table = "VE_STYLE";
                    break;
                case EntityTypeCode.CustomAction:
                    table = "CUSTOM_ACTION";
                    break;
                case EntityTypeCode.BackendAction:
                    table = "BACKEND_ACTION";
                    break;
                case EntityTypeCode.Site:
                    table = "SITE";
                    break;
                case EntityTypeCode.StatusType:
                    table = "STATUS_TYPE";
                    break;
                case EntityTypeCode.Workflow:
                    table = "WORKFLOW";
                    break;
                case EntityTypeCode.Notification:
                    table = "NOTIFICATIONS";
                    break;
            }

            return table;
        }

        internal static void TurnIdentityInsertOn(string entityTypeCode, EntityObject objectForCheck = null)
        {
            if (objectForCheck != null && QPConnectionScope.Current.IdentityInsertOptions.Contains(entityTypeCode))
            {
                objectForCheck.VerifyIdentityInserting(entityTypeCode);
            }

            ChangeIdentityInsertState(entityTypeCode, true);
        }

        internal static void TurnIdentityInsertOff(string entityTypeCode)
        {
            ChangeIdentityInsertState(entityTypeCode, false);
        }

        public static void PostReplay(HashSet<string> insertIdentityOptions)
        {
            using var scope = new QPConnectionScope();
            if (scope.CurrentDbType == DatabaseType.Postgres)
            {
                foreach (var key in insertIdentityOptions)
                {
                    Common.PostgresUpdateSequence(scope.DbConnection, TableToInsert(key), TableIdToInsert(key));
                }
            }
        }
    }
}
