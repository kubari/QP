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
    internal class ContentProfile : Profile
    {
        public ContentProfile()
        {
            CreateMap<ContentDAL, Content>(MemberList.None)
                .ForMember(biz => biz.Fields, opt => opt.Ignore())
                .ForMember(biz => biz.Constraints, opt => opt.Ignore())
                .ForMember(biz => biz.GroupId, opt => opt.MapFrom(dal => dal.GroupId))
                .ForMember(biz => biz.JoinRootId, opt => opt.MapFrom(dal => dal.JoinId))
                .ForMember(biz => biz.StoredVirtualType, opt => opt.MapFrom(dal => dal.VirtualType))
                .ForMember(biz => biz.UserQuery, opt => opt.MapFrom(dal => dal.Query))
                .ForMember(biz => biz.UserQueryAlternative, opt => opt.MapFrom(dal => dal.AltQuery))
                .ForMember(biz => biz.WorkflowBinding, opt => opt.Ignore())
                .ForMember(biz => biz.ParentContent, opt => opt.Ignore())
                .ForMember(biz => biz.ChildContents, opt => opt.Ignore())
                .AfterMap(SetBizProperties);

            CreateMap<Content, ContentDAL>(MemberList.None)
                .ForMember(data => data.GroupId, opt => opt.MapFrom(biz => biz.GroupId))
                .ForMember(data => data.JoinId, opt => opt.MapFrom(biz => biz.JoinRootId))
                .ForMember(data => data.Query, opt => opt.MapFrom(biz => biz.UserQuery))
                .ForMember(data => data.AltQuery, opt => opt.MapFrom(biz => biz.UserQueryAlternative))
                .ForMember(data => data.AccessRules, opt => opt.Ignore())
                .ForMember(data => data.Articles, opt => opt.Ignore())
                .ForMember(data => data.Containers, opt => opt.Ignore())
                .ForMember(data => data.Constraints, opt => opt.Ignore())
                .ForMember(data => data.Fields, opt => opt.Ignore())
                .ForMember(data => data.Folders, opt => opt.Ignore())
                .ForMember(data => data.Group, opt => opt.Ignore())
                .ForMember(data => data.JoinContents, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.LinkedContents, opt => opt.Ignore())
                .ForMember(data => data.Notifications, opt => opt.Ignore())
                .ForMember(data => data.Site, opt => opt.Ignore())
                .ForMember(data => data.UnionContents, opt => opt.Ignore())
                .ForMember(data => data.UnionContents1, opt => opt.Ignore())
                .ForMember(data => data.UnionContents2, opt => opt.Ignore())
                .ForMember(data => data.UserQueryAttrs, opt => opt.Ignore())
                .ForMember(data => data.UserQueryContents, opt => opt.Ignore())
                .ForMember(data => data.UserQueryContents1, opt => opt.Ignore())
                .ForMember(data => data.WorkflowBinding, opt => opt.Ignore())
                .ForMember(data => data.CustomActions, opt => opt.Ignore())
                .ForMember(data => data.ParentContent, opt => opt.Ignore())
                .ForMember(data => data.ChildContents, opt => opt.Ignore())
                .AfterMap(SetDalProperties);

             CreateMap<ContentWorkflowBindDAL, ContentWorkflowBind>(MemberList.None)
                .ForMember(biz => biz.Content, opt => opt.Ignore())
                ;

             CreateMap<ContentWorkflowBind, ContentWorkflowBindDAL>(MemberList.None)
                 .ForMember(data => data.Content, opt => opt.Ignore())
                 ;

             CreateMap<ContentGroupDAL, Content>(MemberList.None);

             CreateMap<ContentGroup, ContentGroupDAL>(MemberList.None)
                 .ForMember(data => data.Site, opt => opt.Ignore())
                 .ForMember(data => data.Contents, opt => opt.Ignore())
                 ;

             CreateMap<ContentToContentDAL, ContentLink>(MemberList.None)
                 .ForMember(biz => biz.Content, opt => opt.Ignore())
                 ;

             CreateMap<ContentLink, ContentToContentDAL>(MemberList.None)
                 .ForMember(data => data.Content, opt => opt.Ignore())
                 ;

             CreateMap<ContentFormDAL, ContentForm>(MemberList.None)
                 .ForMember(data => data.GenerateUpdateScript, opt => opt.MapFrom(x => Converter.ToBoolean(x.GenerateUpdateScript, false)))
                 .ForMember(data => data.Page, opt => opt.Ignore())
                 ;

             CreateMap<ContentForm, ContentFormDAL>(MemberList.None)
                 .ForMember(x => x.GenerateUpdateScript, opt => opt.MapFrom(src => src.GenerateUpdateScript ? 1 : 0))
                 .ForMember(x => x.Content, opt => opt.Ignore())
                 .ForMember(x => x.Object, opt => opt.Ignore())
                 .ForMember(x => x.Page, opt => opt.Ignore())
                 .ForMember(x => x.LockedBy, opt => opt.Ignore())
                 .ForMember(x => x.Locked, opt => opt.Ignore());

             CreateMap<ContentConstraintRuleDAL, ContentConstraintRule>(MemberList.None)
                 .ForMember(biz => biz.Field, opt => opt.Ignore());

             CreateMap<ContentConstraintRule, ContentConstraintRuleDAL>(MemberList.None)
                 .ForMember(data => data.Field, opt => opt.Ignore());

             CreateMap<ContentFolderDAL, ContentFolder>(MemberList.None)
                 .ForMember(biz => biz.StoredPath, opt => opt.MapFrom(data => data.Path));

             CreateMap<ContentFolder, ContentFolderDAL>(MemberList.None)
                 .ForMember(data => data.Content, opt => opt.Ignore())
                 .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore());

             CreateMap<DataRow, ContentFolder>(MemberList.None)
                 .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("FOLDER_ID"))))
                 .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                 .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>(FieldName.LastModifiedBy))))
                 .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                 .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                 .ForMember(biz => biz.HasChildren, opt => opt.MapFrom(row => row.Field<bool>("HAS_CHILDREN")))
                 .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => SiteFolderProfile.GetModifierUserFromRow(row)));


             CreateMap<DataRow, ContentListItem>(MemberList.None)
                 .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                 .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                 .ForMember(biz => biz.GroupName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("GroupName"), ContentStrings.DefaultContentGroup)))
                 .ForMember(biz => biz.SiteName, opt => opt.MapFrom(row => row.Field<string>("SiteName")))
                 .ForMember(biz => biz.Description, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("Description"), string.Empty)))
                 .ForMember(biz => biz.VirtualType, opt => opt.MapFrom(row => Content.GetVirtualTypeString(Converter.ToInt32(row.Field<decimal>("VirtualType")))))
                 .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                 .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                 .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByUser")))
                 .AfterMap(SetBizRowProperties);

        }

        private static void SetBizProperties(ContentDAL dataObject, Content bizObject)
        {
            if (dataObject is { MaxNumOfStoredVersions: 0 })
            {
                bizObject.MaxNumOfStoredVersions = Content.DefaultLimitOfStoredVersions;
                bizObject.UseVersionControl = false;
            }
        }

        private static void SetDalProperties(Content bizObject, ContentDAL dataObject)
        {
            if (bizObject is { UseVersionControl: false })
            {
                dataObject.MaxNumOfStoredVersions = 0;
            }
        }

        private static void SetBizRowProperties(DataRow dal, ContentListItem biz)
        {
            if (biz.GroupName == ContentGroup.DefaultName)
            {
                biz.GroupName = ContentGroup.TranslatedDefaultName;
            }
        }
    }
}
