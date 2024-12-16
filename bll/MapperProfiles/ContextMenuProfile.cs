using System.Data;
using System.Linq;
using AutoMapper;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ContextMenuProfile : Profile
    {
        public ContextMenuProfile()
        {
            CreateMap<ContextMenuItemDAL, ContextMenuItem>(MemberList.None)
                .ForMember(biz => biz.BottomSeparator, opt => opt.MapFrom(x => x.HasBottomSeparator))
                .ForMember(biz => biz.ActionCode, opt => opt.MapFrom(x => x.Action != null ? x.Action.Code : null))
                .ForMember(biz => biz.ActionTypeCode, opt => opt.MapFrom(x => x.Action != null && x.Action.ActionType != null ? x.Action.ActionType.Code : null));

            CreateMap<ContextMenuItem, ContextMenuItemDAL>(MemberList.None);

            CreateMap<ContextMenuDAL, ContextMenu>(MemberList.None)
                .ForMember(biz => biz.Items, opt => opt.MapFrom(x => QPContext.Map<ContextMenuItem[]>(x.Items.ToList())));

            CreateMap<DataRow, ContextMenu>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => row.Field<int>("ID")))
                .ForMember(biz => biz.Code, opt => opt.MapFrom(row => row.Field<string>("CODE")))
                .ForMember(biz => biz.Items, opt => opt.MapFrom(row => QPContext.Map<ContextMenuItem[]>(row.GetChildRows("Menu2Item").ToList())));

            CreateMap<DataRow, ContextMenuItem>(MemberList.None)
                .ForMember(biz => biz.ActionCode, opt => opt.MapFrom(row => row.Field<string>("ACTION_CODE")))
                .ForMember(biz => biz.ActionTypeCode, opt => opt.MapFrom(row => row.Field<string>("ACTION_TYPE_CODE")))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.Icon, opt => opt.MapFrom(row => row.Field<string>("ICON")))
                .ForMember(biz => biz.BottomSeparator, opt => opt.MapFrom(row => row.Field<bool>("BOTTOM_SEPARATOR")));

        }
    }
}
