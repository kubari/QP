using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.BLL.Helpers;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.BLL.Repository.ContentRepositories;
using Quantumart.QP8.BLL.Services.DTO;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class ObjectRepository
    {
        internal static IEnumerable<ObjectListItem> ListTemplateObjects(ListCommand cmd, int templateId, out int totalRecords)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetTemplateObjectsByTemplateId(scope.DbConnection, templateId, cmd.SortExpression, out totalRecords, cmd.StartRecord, cmd.PageSize);
            return QPContext.Map<ObjectListItem[]>(rows.ToList());
        }

        internal static IEnumerable<ObjectListItem> ListPageObjects(ListCommand cmd, int parentId, out int totalRecords)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.GetPageObjectsByPageId(scope.DbConnection, parentId, cmd.SortExpression, out totalRecords, cmd.StartRecord, cmd.PageSize);
            return QPContext.Map<ObjectListItem[]>(rows.ToList());
        }

        internal static BllObject SaveObjectProperties(BllObject bllObject)
        {
            var result = DefaultRepository.Save<BllObject, ObjectDAL>(bllObject);
            if (bllObject.IsObjectContainerType)
            {
                bllObject.Container.ObjectId = result.Id;
                result.Container = SaveContainer(bllObject.Container);
            }
            else if (bllObject.IsObjectFormType)
            {
                bllObject.ContentForm.ObjectId = result.Id;
                result.ContentForm = SaveContentForm(bllObject.ContentForm);
            }

            if (bllObject.DefaultValues != null && bllObject.DefaultValues.Any())
            {
                SetDefaultValues(bllObject.DefaultValues, result.Id);
            }

            return result;
        }

        internal static void SetDefaultValues(IEnumerable<DefaultValue> dvals, int objectId)
        {
            var entities = QPContext.EFContext;
            foreach (var obj in dvals)
            {
                var dal = new ObjectValuesDAL() { ObjectId = objectId, VariableName = obj.VariableName, VariableValue = obj.VariableValue};
                entities.Entry(dal).State = EntityState.Added;
            }

            entities.SaveChanges();
        }

        private static void DeleteDefaultValues(int objectId)
        {
            var entities = QPContext.EFContext;
            foreach (var dal in entities.ObjectValuesSet.Where(x => x.ObjectId == objectId))
            {
                entities.Entry(dal).State = EntityState.Deleted;
            }

            entities.SaveChanges();
        }

        internal static IEnumerable<ObjectValue> GetDefaultValuesByObjectId(int objectId)
        {
            var entities = QPContext.EFContext;
            return QPContext.Map<ObjectValue[]>(entities.ObjectValuesSet.Where(x => x.ObjectId == objectId).ToList());
        }

        internal static BllObject UpdateObjectProperties(BllObject bllObject)
        {
            var oldObject = GetObjectPropertiesById(bllObject.Id);
            var result = DefaultRepository.Update<BllObject, ObjectDAL>(bllObject);
            ManageObjectType(bllObject, oldObject, result);

            DeleteDefaultValues(result.Id);
            SetDefaultValues(bllObject.DefaultValues, result.Id);
            return result;
        }

        internal static void UpdateDefaultFormatId(int objectId, int formatId)
        {
            using var scope = new QPConnectionScope();
            Common.UpdateDefaultFormatId(scope.DbConnection, objectId, formatId);
        }

        private static void ManageObjectType(BllObject newObject, BllObject oldObject, BllObject result)
        {
            if (newObject.TypeId != oldObject.TypeId) //object type was changed
            {
                if (oldObject.IsObjectContainerType)
                {
                    DeleteContainer(oldObject.Id);
                }
                else if (oldObject.IsObjectFormType)
                {
                    DeleteForm(oldObject.Id);
                }

                if (newObject.IsObjectContainerType)
                {
                    newObject.Container.ObjectId = result.Id;
                    result.Container = SaveContainer(newObject.Container);

                    // ReSharper disable once PossibleInvalidOperationException
                    result.Container.Content = ContentRepository.GetById(result.Container.ContentId.Value);
                }

                else if (newObject.IsObjectFormType)
                {
                    newObject.ContentForm.ObjectId = result.Id;
                    result.ContentForm = SaveContentForm(newObject.ContentForm);

                    // ReSharper disable once PossibleInvalidOperationException
                    result.ContentForm.Content = ContentRepository.GetById(result.ContentForm.ContentId.Value);
                }
            }

            else // just update form or container
            {
                if (newObject.IsObjectFormType)
                {
                    result.ContentForm = oldObject.ContentForm.ContentId == null ? SaveContentForm(newObject.ContentForm) : UpdateForm(newObject.ContentForm);
                }
                if (newObject.IsObjectContainerType)
                {
                    result.Container = oldObject.Container.ContentId == null ? SaveContainer(newObject.Container) : UpdateContainer(newObject.Container);
                }
            }
        }

        internal static BllObject GetObjectPropertiesById(int id)
        {
            var result = QPContext.Map<BllObject>(QPContext.EFContext.ObjectSet.Include("ChildObjectFormats.Notifications").Include("InheritedObjects")
                .Include("PageTemplate.Site").Include("ObjectType").Include("LastModifiedByUser")
                .SingleOrDefault(g => g.Id == id));
            result.DefaultValues = GetDefaultValuesByObjectId(id).Select(x => new DefaultValue { VariableName = x.VariableName, VariableValue = x.VariableValue });
            return result;
        }

        internal static void DeleteObject(int id)
        {
            DefaultRepository.Delete<ObjectDAL>(id);
        }

        internal static void SetPageObjectEnableViewState(int pageId, bool enableViewState)
        {
            var entities = QPContext.EFContext;

            var objects = entities.ObjectSet.Where(x => x.PageId == pageId);
            foreach (var obj in objects)
            {
                obj.EnableViewstate = enableViewState;
                /*using (new QPConnectionScope())
				{
					obj.Modified = Common.GetSqlDate(QPConnectionScope.Current.DbConnection);
				}*/
            }

            entities.SaveChanges();
        }

        internal static bool ObjectNetNameUnique(string netName, int? pageId, int pageTemplateId, bool pageOrTemplate, int id)
        {
            var entities = QPContext.EFContext;
            return !entities.ObjectSet.Any(x =>
                (pageOrTemplate && x.PageId == pageId || !pageOrTemplate && x.PageId == null && x.PageTemplateId == pageTemplateId)
                && x.NetName == netName && x.Id != id);
        }

        internal static Container SaveContainer(Container container)
        {
            var entities = QPContext.EFContext;
            container.CursorType = "adOpenForwardOnly";
            container.CursorLocation = "adUseClient";
            var dal = QPContext.Map<ContainerDAL>(container);
            dal.CursorType = "adOpenForwardOnly";
            dal.CursorLocation = "adUseClient";
            dal.LockType = "adLockReadOnly";
            dal.Locked = null;
            entities.Entry(dal).State = EntityState.Added;
            entities.SaveChanges();
            return QPContext.Map<Container>(dal);
        }

        private static Container UpdateContainer(Container container)
        {
            var dal = QPContext.Map<ContainerDAL>(container);
            dal = DefaultRepository.SimpleUpdate(dal);
            return QPContext.Map<Container>(dal);
        }

        private static ContentForm UpdateForm(ContentForm form)
        {
            var dal = QPContext.Map<ContentFormDAL>(form);
            dal = DefaultRepository.SimpleUpdate(dal);
            return QPContext.Map<ContentForm>(dal);
        }

        internal static ContentForm SaveContentForm(ContentForm contentForm)
        {
            var entities = QPContext.EFContext;
            var dal = QPContext.Map<ContentFormDAL>(contentForm);
            entities.Entry(dal).State = EntityState.Added;
            entities.SaveChanges();
            return QPContext.Map<ContentForm>(dal);
        }

        private static void DeleteContainer(int objectId)
        {
            using var scope = new QPConnectionScope();
            Common.DeleteObjectContainer(scope.DbConnection, objectId);
        }

        private static void DeleteForm(int objectId)
        {
            using var scope = new QPConnectionScope();
            Common.DeleteObjectForm(scope.DbConnection, objectId);
        }

        internal static void SetObjectActiveStatuses(int objectId, IEnumerable<int> activeStatuses, bool isContainer)
        {
            using var scope = new QPConnectionScope();
            Common.CleanObjectActiveStatuses(scope.DbConnection, objectId);
            if (isContainer)
            {
                Common.SetObjectActiveStatuses(scope.DbConnection, objectId, activeStatuses);
            }
        }

        internal static IEnumerable<int> GetObjectActiveStatusIds(int objectId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetObjectActiveStatusesIds(scope.DbConnection, objectId);
        }

        internal static IEnumerable<BllObject> GetTemplateObjects(int templateId)
        {
            var entities = QPContext.EFContext;
            return QPContext.Map<BllObject[]>(entities.ObjectSet.Include("ChildObjectFormats").Include("PageTemplate").Where(x => x.PageTemplateId == templateId && x.PageId == null).ToList());
        }

        internal static IEnumerable<BllObject> GetPageObjects(int pageId)
        {
            var entities = QPContext.EFContext;
            return QPContext.Map<BllObject[]>(entities.ObjectSet.Include("ChildObjectFormats").Include("PageTemplate").Where(x => x.PageId == pageId).ToList());
        }

        internal static IEnumerable<TemplateObjectFormatDto> GetRestTemplateObjects(int templateId)
        {
            using var scope = new QPConnectionScope();
            var siteId = PageTemplateRepository.GetPageTemplatePropertiesById(templateId).SiteId;
            return QPContext.Map<TemplateObjectFormatDto[]>(Common.GetRestTemplateObjects(scope.DbConnection, templateId, siteId).ToList());
        }

        internal static IEnumerable<EntityObject> GetList(IEnumerable<int> ids)
        {
            IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
            return QPContext.Map<BllObject[]>(
                QPContext.EFContext.ObjectSet
                    .Where(f => decIDs.Contains(f.Id))
                    .ToList()
            );
        }

        internal static int GetTemplatesElementsCountOnSite(int siteId)
        {
            using var scope = new QPConnectionScope();
            return Common.GetTemplatesElementsCountOnSite(scope.DbConnection, siteId);
        }

        internal static void CopySiteTemplateObjects(string relationsBetweenTemplates, string relationsBetweenPages, out string result)
        {
            using var scope = new QPConnectionScope();
            var rows = Common.CopySiteTemplateObjects(scope.DbConnection, relationsBetweenTemplates, relationsBetweenPages);
            result = MultistepActionHelper.GetXmlFromDataRows(rows, "object");
        }

        internal static void CopySiteUpdateObjects(string relationsBetweenObjectFormats, string relationsBetweenObjects)
        {
            using var scope = new QPConnectionScope();
            Common.CopySiteUpdateObjects(scope.DbConnection, relationsBetweenObjectFormats, relationsBetweenObjects);
        }

        internal static void CopySiteObjectValues(string relationsBetweenObjects)
        {
            using var scope = new QPConnectionScope();
            Common.CopySiteObjectValues(scope.DbConnection, relationsBetweenObjects);
        }

        internal static void CopySiteContainers(string relationsBetweenObjects, string relationsBetweenContents)
        {
            using var scope = new QPConnectionScope();
            Common.CopySiteContainers(scope.DbConnection, relationsBetweenObjects, relationsBetweenContents);
        }

        internal static void CopyContainerStatuses(string relBetweenStatuses, string relBetweenObjects)
        {
            using var scope = new QPConnectionScope();
            Common.CopyContainerStatuses(scope.DbConnection, relBetweenStatuses, relBetweenObjects);
        }
    }
}
