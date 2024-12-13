using AutoMapper;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class EntityTypeProfile : Profile
    {
        public EntityTypeProfile()
        {
            CreateMap<EntityTypeDAL, EntityType>(MemberList.None)
                .ForMember(biz => biz.ParentCode, opt => opt.MapFrom(src => src.Parent != null ? src.Parent.Code : null))
                .ForMember(biz => biz.CancelActionCode, opt => opt.MapFrom(src => src.CancelAction != null ? src.CancelAction.Code : null))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(data => Translator.Translate(data.Name)))
                .ForMember(biz => biz.NotTranslatedName, opt => opt.MapFrom(data => data.Name))
                .ForMember(biz => biz.TabId, opt => opt.MapFrom(data => Converter.ToNullableInt32(data.TabId)))
                ;

            CreateMap<EntityType, EntityTypeDAL>(MemberList.None)
                .ForMember(data => data.FolderIcon, opt => opt.Ignore())
                .ForMember(data => data.FolderDefaultActionId, opt => opt.Ignore())
                .ForMember(data => data.FolderContextMenuId, opt => opt.Ignore())
                .ForMember(data => data.ACTION_PERMISSION_ENABLE, opt => opt.Ignore())
                .ForMember(data => data.Actions, opt => opt.Ignore())
                .ForMember(data => data.DefaultAction, opt => opt.Ignore())
                .ForMember(data => data.FolderDefaultAction, opt => opt.Ignore())
                .ForMember(data => data.Children, opt => opt.Ignore())
                ;
        }
    }
}
