using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDAL>(MemberList.None)
                .ForMember(data => data.LanguageId, opt => opt.MapFrom(src => src.LanguageId))
                .ForMember(data => data.AccessRules, opt => opt.Ignore())
                .ForMember(data => data.Container, opt => opt.Ignore())
                .ForMember(data => data.Content, opt => opt.Ignore())
                .ForMember(data => data.ContentAccess, opt => opt.Ignore())
                .ForMember(data => data.ContentFolderAccess, opt => opt.Ignore())
                .ForMember(data => data.ContentForm, opt => opt.Ignore())
                .ForMember(data => data.FolderAccess, opt => opt.Ignore())
                .ForMember(data => data.Groups, opt => opt.Ignore())
                .ForMember(data => data.Languages, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedSites, opt => opt.Ignore())
                .ForMember(data => data.LockedByArticles, opt => opt.Ignore())
                .ForMember(data => data.LockedSites, opt => opt.Ignore())
                .ForMember(data => data.Notifications, opt => opt.Ignore())
                .ForMember(data => data.NotificationsForBackendUser, opt => opt.Ignore())
                .ForMember(data => data.Object, opt => opt.Ignore())
                .ForMember(data => data.ObjectFormat, opt => opt.Ignore())
                .ForMember(data => data.Page, opt => opt.Ignore())
                .ForMember(data => data.PageTemplate, opt => opt.Ignore())
                .ForMember(data => data.SiteAccess, opt => opt.Ignore())
                .ForMember(data => data.UserToPanel, opt => opt.Ignore())
                .ForMember(data => data.WaitingForApproval, opt => opt.Ignore())
                .ForMember(data => data.WorkflowAccess, opt => opt.Ignore())
                .ForMember(data => data.WorkflowRules, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.STATUS_TYPE, opt => opt.Ignore())
                .ForMember(data => data.LastLogOn, opt => opt.MapFrom(biz => biz.LastLogOn == default(DateTime) ? null : biz.LastLogOn))
       ;
            CreateMap<UserDAL, User>(MemberList.None);

            CreateMap<DataRow, UserListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.Login, opt => opt.MapFrom(row => row.Field<string>("LOGIN")))
                .ForMember(biz => biz.FirstName, opt => opt.MapFrom(row => row.Field<string>("FIRST_NAME")))
                .ForMember(biz => biz.LastName, opt => opt.MapFrom(row => row.Field<string>("LAST_NAME")))
                .ForMember(biz => biz.Email, opt => opt.MapFrom(row => row.Field<string>("EMAIL")))
                .ForMember(biz => biz.LanguageId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LANGUAGE_ID"))))
                .ForMember(biz => biz.Language, opt => opt.MapFrom(row => Translator.Translate(row.Field<string>("LANGUAGE_NAME"))))
                .ForMember(biz => biz.LastLogOn, opt => opt.MapFrom(row => row.Field<DateTime?>("LAST_LOGIN")))
                .ForMember(biz => biz.Disabled, opt => opt.MapFrom(row => Converter.ToBoolean(row.Field<decimal>("DISABLED"))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.LastModifiedByUserId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>(FieldName.LastModifiedBy))))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LAST_MODIFIED_BY_LOGIN")));

        }
    }
}
