using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ToolbarButtonProfile : Profile
    {
        public ToolbarButtonProfile()
        {
            CreateMap<ToolbarButton, ToolbarButtonDAL>(MemberList.None)
                .ForMember(data => data.Action, opt => opt.Ignore())
                .ForMember(data => data.ParentAction, opt => opt.Ignore());

            CreateMap<ToolbarButtonDAL, ToolbarButton>(MemberList.None);

            CreateMap<DataRow, ToolbarButton>(MemberList.None)
                .ForMember(biz => biz.ActionId, opt => opt.MapFrom(row => row.Field<int>("ACTION_ID")))
                .ForMember(biz => biz.ActionCode, opt => opt.MapFrom(row => row.Field<string>("ACTION_CODE")))
                .ForMember(biz => biz.ActionTypeCode, opt => opt.MapFrom(row => row.Field<string>("ACTION_TYPE_CODE")))
                .ForMember(biz => biz.ParentActionId, opt => opt.MapFrom(row => row.Field<int>("PARENT_ACTION_ID")))
                .ForMember(biz => biz.ParentActionCode, opt => opt.MapFrom(row => row.Field<string>("PARENT_ACTION_CODE")))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.ItemsAffected, opt => opt.MapFrom(row => Convert.ToByte(row["ITEMS_AFFECTED"])))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => row.Field<int>("ORDER")))
                .ForMember(biz => biz.Icon, opt => opt.MapFrom(row => row.Field<string>("ICON")))
                .ForMember(biz => biz.IconDisabled, opt => opt.MapFrom(row => row.Field<string>("ICON_DISABLED")))
                .ForMember(biz => biz.IsCommand, opt => opt.MapFrom(row => row.Field<bool>("IS_COMMAND")));

        }
    }
}
