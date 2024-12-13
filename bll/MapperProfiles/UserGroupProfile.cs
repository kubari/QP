using System;
using System.Data;
using System.Linq;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class UserGroupProfile : Profile
    {
        public UserGroupProfile()
        {
            CreateMap<UserGroup, UserGroupDAL>(MemberList.None)
                .ForMember(dal => dal.ContentAccess, opt => opt.Ignore())
                .ForMember(dal => dal.ContentFolderAccess, opt => opt.Ignore())
                .ForMember(dal => dal.ArticleAccess, opt => opt.Ignore())
                .ForMember(dal => dal.FolderAccess, opt => opt.Ignore())
                .ForMember(dal => dal.Notifications, opt => opt.Ignore())
                .ForMember(dal => dal.SiteAccess, opt => opt.Ignore())
                .ForMember(dal => dal.WorkflowAccess, opt => opt.Ignore())
                .ForMember(dal => dal.WorkflowRules, opt => opt.Ignore())
                .ForMember(dal => dal.Users, opt => opt.Ignore())
                .ForMember(dal => dal.ChildGroups, opt => opt.Ignore())
                .ForMember(dal => dal.ParentGroups, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore());

            CreateMap<UserGroupDAL, UserGroup>(MemberList.None)
                .ForMember(biz => biz.ParentGroup, opt => opt.MapFrom(
                    data => data.ParentGroups != null && data.ParentGroups.Any() ? data.ParentGroups.FirstOrDefault() : null)
                );

            CreateMap<DataRow, UserGroupListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.SharedArticles, opt => opt.MapFrom(row => Converter.ToBoolean(row.Field<decimal>("SharedArticles"))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.LastModifiedByUserId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedByUserId"))))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByUser")));

        }
    }
}
