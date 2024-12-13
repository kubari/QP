using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class StatusHistoryProfile : Profile
    {
        public StatusHistoryProfile()
        {
            CreateMap<DataRow, StatusHistoryListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Comment, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("Comment"))))
                .ForMember(biz => biz.ActionMadeBy, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("ActionMadeBy"))))
                .ForMember(biz => biz.ActionDate, opt => opt.MapFrom(row => Converter.ToDateTime(row.Field<DateTime>("ActionDate"))))
                .ForMember(biz => biz.SystemStatusTypeName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("SystemStatusTypeName"))))
                .ForMember(biz => biz.Version, opt => opt.MapFrom(row => Converter.ToString(row.Field<decimal?>("Version"))))
                .ForMember(biz => biz.StatusTypeName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("StatusTypeName"))));
        }
    }
}
