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
    internal class CustomActionProfile : Profile
    {
        public CustomActionProfile()
        {
            CreateMap<CustomAction, CustomActionDAL>(MemberList.None)
                .ForMember(data => data.Action, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Sites, opt => opt.Ignore())
                .ForMember(data => data.Contents, opt => opt.Ignore());

            CreateMap<CustomActionDAL, CustomAction>(MemberList.None)
                .ForMember(data => data.ContentIds, opt => opt.MapFrom(
                    src => src.ContentCustomActionBinds.Select(n => n.ContentId).ToArray()))
                .ForMember(data => data.SiteIds, opt => opt.MapFrom(
                    src => src.SiteCustomActionBinds.Select(n => n.SiteId).ToArray()))
                .ForMember(data => data.Action, opt => opt.Ignore())
                ;

            CreateMap<DataRow, CustomActionListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.Url, opt => opt.MapFrom(row => row.Field<string>("URL")))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => row.Field<int>("ORDER")))
                .ForMember(biz => biz.ActionTypeName, opt => opt.MapFrom(row => Translator.Translate(row.Field<string>("ACTION_TYPE_NAME"))))
                .ForMember(biz => biz.EntityTypeName, opt => opt.MapFrom(row => Translator.Translate(row.Field<string>("ENTITY_TYPE_NAME"))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.LastModifiedByUserId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("USER_ID"))))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LOGIN")));

        }
    }
}
