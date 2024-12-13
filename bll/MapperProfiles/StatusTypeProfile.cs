using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class StatusTypeProfile : Profile
    {
        public StatusTypeProfile()
        {
            CreateMap<StatusType, StatusTypeDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.WorkflowRules, opt => opt.Ignore())
                ;

            CreateMap<StatusTypeDAL, StatusType>(MemberList.None);

            CreateMap<DataRow, StatusTypeListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Weight, opt => opt.MapFrom(row => row.Field<decimal>("Weight")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                ;

            CreateMap<DataRow, StatusType>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>(FieldName.StatusTypeId))))
                .ForMember(biz => biz.Weight, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("WEIGHT"))))
                .ForMember(biz => biz.SiteId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("SITE_ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>(FieldName.StatusTypeName)))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("DESCRIPTION")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>(FieldName.LastModifiedBy))))
                .ForMember(biz => biz.BuiltIn, opt => opt.MapFrom(row => Converter.ToBoolean(row.Field<bool>("BUILT_IN"))));

        }
    }
}
