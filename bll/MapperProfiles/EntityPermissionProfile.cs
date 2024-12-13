using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Resources;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    public class EntityPermissionProfile : Profile
    {
        public EntityPermissionProfile()
        {
            CreateActionPermissionMaps();
            CreateArticlePermissionMaps();
            CreateEntityTypePermissionMaps();
            CreateContentPermissionMaps();
            CreateSitePermissionMaps();
            CreateSiteFolderPermissionMaps();
            CreateWorkflowPermissionMaps();

            CreateMap<DataRow, ChildEntityPermissionListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.IsExplicit, opt => opt.MapFrom(row => row.Field<bool>("IsExplicit")))
                .ForMember(biz => biz.Hide, opt => opt.MapFrom(row => row.Field<bool>("Hide")))
                .ForMember(biz => biz.PropagateToItems, opt => opt.MapFrom(row => Converter.ToBoolean(row.Field<decimal>("PropagateToItems"))))
                .ForMember(biz => biz.Title, opt => opt.MapFrom(row => row.Field<string>("Title")))
                .ForMember(biz => biz.LevelName, opt =>
                    opt.MapFrom(row => Translator.Translate(row.Field<string>("LevelName")) ?? EntityPermissionStrings.UndefinedPermissionLevel)
                );

            CreateMap<DataRow, EntityPermissionListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.UserLogin, opt => opt.MapFrom(row => row.Field<string>("UserLogin")))
                .ForMember(biz => biz.GroupName, opt => opt.MapFrom(row => row.Field<string>("GroupName")))
                .ForMember(biz => biz.LevelName, opt => opt.MapFrom(row => Translator.Translate(row.Field<string>("LevelName"))))
                .ForMember(biz => biz.PropagateToItems, opt => opt.MapFrom(row => Converter.ToBoolean(row.Field<decimal>("PropagateToItems"))))
                .ForMember(biz => biz.Hide, opt => opt.MapFrom(row => row.Field<bool>("Hide")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.LastModifiedByUserId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedByUserId"))))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByUser")));
        }

        private void CreateArticlePermissionMaps()
        {
            CreateMap<ArticlePermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<Article>(data.Article)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => Converter.ToInt32(data.ArticleId)));

            CreateMap<EntityPermission, ArticlePermissionDAL>(MemberList.None)
                .ForMember(data => data.Article, opt => opt.Ignore())
                .ForMember(data => data.ArticleId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.ParentEntityId)))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }

        private void CreateActionPermissionMaps()
        {
            CreateMap<BackendActionPermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<BackendAction>(data.Action)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => data.ActionId));

            CreateMap<EntityPermission, BackendActionPermissionDAL>(MemberList.None)
                .ForMember(data => data.Action, opt => opt.Ignore())
                .ForMember(data => data.ActionId, opt => opt.MapFrom(biz => biz.ParentEntityId))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());

        }

        private void CreateEntityTypePermissionMaps()
        {
            CreateMap<EntityTypePermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<EntityType>(data.EntityType)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => data.EntityTypeId));

            CreateMap<EntityPermission, EntityTypePermissionDAL>(MemberList.None)
                .ForMember(data => data.EntityType, opt => opt.Ignore())
                .ForMember(data => data.EntityTypeId, opt => opt.MapFrom(biz => biz.ParentEntityId))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }

        private void CreateContentPermissionMaps()
        {
            CreateMap<ContentPermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<Content>(data.Content)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => Converter.ToInt32(data.ContentId)));

            CreateMap<EntityPermission, ContentPermissionDAL>(MemberList.None)
                .ForMember(data => data.Content, opt => opt.Ignore())
                .ForMember(data => data.ContentId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.ParentEntityId)))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }

        private void CreateSiteFolderPermissionMaps()
        {
            CreateMap<SiteFolderPermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<SiteFolder>(data.Folder)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => Converter.ToInt32(data.FolderId)));

            CreateMap<EntityPermission, SiteFolderPermissionDAL>(MemberList.None)
                .ForMember(data => data.Folder, opt => opt.Ignore())
                .ForMember(data => data.FolderId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.ParentEntityId)))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }

        private void CreateSitePermissionMaps()
        {
            CreateMap<SitePermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<Site>(data.Site)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => Converter.ToInt32(data.SiteId)));

            CreateMap<EntityPermission, SitePermissionDAL>(MemberList.None)
                .ForMember(data => data.Site, opt => opt.Ignore())
                .ForMember(data => data.SiteId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.ParentEntityId)))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }

        private void CreateWorkflowPermissionMaps()
        {
            CreateMap<WorkflowPermissionDAL, EntityPermission>(MemberList.None)
                .ForMember(biz => biz.Parent, opt => opt.MapFrom(data => QPContext.Map<Workflow>(data.Workflow)))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(data => Converter.ToInt32(data.WorkflowId)));

            CreateMap<EntityPermission, WorkflowPermissionDAL>(MemberList.None)
                .ForMember(data => data.Workflow, opt => opt.Ignore())
                .ForMember(data => data.WorkflowId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.ParentEntityId)))
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.PermissionLevel, opt => opt.Ignore())
                .ForMember(data => data.User, opt => opt.Ignore());
        }
    }
}
