using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ButtonTraceProfile : Profile
    {
        public ButtonTraceProfile()
        {
            CreateMap<DataRow, ButtonTrace>(MemberList.None)
                .ForMember(biz => biz.ButtonName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("ButtonName"), string.Empty)))
                .ForMember(biz => biz.TabName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("TabName"), string.Empty)))
                .ForMember(biz => biz.ActivatedTime, opt => opt.MapFrom(row => row.Field<DateTime>("ActivatedTime")))
                .ForMember(biz => biz.UserId, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("UserId"))))
                .ForMember(biz => biz.UserLogin, opt => opt.MapFrom(row => row.Field<string>("UserLogin")));
        }
    }
}
