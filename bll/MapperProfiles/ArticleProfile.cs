using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleDAL, Article>(MemberList.None)
                .ForMember(biz => biz.LockedBy, opt => opt.MapFrom(src => Convert.ToInt32(src.LockedBy)))
                .ForMember(data => data.WorkflowBinding, opt => opt.Ignore());

            CreateMap<Article, ArticleDAL>(MemberList.None)
                .ForMember(data => data.Locked, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (DateTime?)src.Locked))
                .ForMember(data => data.LockedBy, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (int?)src.LockedBy))
                .ForMember(data => data.AccessRules, opt => opt.Ignore())
                .ForMember(data => data.Content, opt => opt.Ignore())
                .ForMember(data => data.ContentData, opt => opt.Ignore())
                .ForMember(data => data.ItemToItem, opt => opt.Ignore())
                .ForMember(data => data.ItemToItemVersions, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.LockedByUser, opt => opt.Ignore())
                .ForMember(data => data.Schedules, opt => opt.Ignore())
                .ForMember(data => data.Status, opt => opt.Ignore())
                .ForMember(data => data.StatusHistory, opt => opt.Ignore())
                .ForMember(data => data.Versions, opt => opt.Ignore())
                .ForMember(data => data.WorkflowBinding, opt => opt.Ignore());

            CreateMap<DataRow, ArticleListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Convert.ToInt32(row["ID"])))
                .ForMember(biz => biz.ParentId, opt => opt.MapFrom(row => Convert.ToInt32(row["ParentId"])))
                .ForMember(biz => biz.StatusName, opt => opt.MapFrom(row => row.Field<string>("StatusName")))
                .ForMember(biz => biz.SiteName, opt => opt.MapFrom(row => row.Field<string>("SiteName")))
                .ForMember(biz => biz.ContentName, opt => opt.MapFrom(row => row.Field<string>("ContentName")))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByUser")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.Title, opt => opt.MapFrom(row => row.Field<string>("Title")))
                .ForMember(biz => biz.IsPermanentLock, opt => opt.MapFrom(row => row.Field<bool>("IsPermanentLock")))
                ;

            CreateMap<DataRow, Article>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Convert.ToInt32(row[FieldName.ContentItemId])))
                .ForMember(biz => biz.StatusTypeId, opt => opt.MapFrom(row => Convert.ToInt32(row[FieldName.StatusTypeId])))
                .ForMember(biz => biz.Visible, opt => opt.MapFrom(row => Converter.ToBoolean(row[FieldName.Visible])))
                .ForMember(biz => biz.Archived, opt => opt.MapFrom(row => Converter.ToBoolean(row[FieldName.Archive])))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Convert.ToInt32(row[FieldName.LastModifiedBy])))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)));


            CreateMap<ArticleWorkflowBindDAL, ArticleWorkflowBind>(MemberList.None)
                .ForMember(biz => biz.Article, opt => opt.Ignore())
                ;

            CreateMap<ArticleWorkflowBind, ArticleWorkflowBindDAL>(MemberList.None)
                .ForMember(data => data.Article, opt => opt.Ignore())
                ;
        }
    }
}
