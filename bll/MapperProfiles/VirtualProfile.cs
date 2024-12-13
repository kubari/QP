using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.Repository.Results;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class VirtualProfile : Profile
    {
        public VirtualProfile()
        {
            CreateMap<UserQueryAttrsDAL, UserQueryAttr>(MemberList.None)
                .ForMember(biz => biz.BaseFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.UserQueryAttrId)))
                .ForMember(biz => biz.UserQueryContentId, opt => opt.MapFrom(r => Converter.ToInt32(r.VirtualContentId)));

            CreateMap<UserQueryAttr, UserQueryAttrsDAL>(MemberList.None)
                .ForMember(data => data.UserQueryAttrId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.BaseFieldId)))
                .ForMember(data => data.VirtualContentId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.UserQueryContentId)));

            CreateMap<DataRow, VirtualFieldData>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("Id"))))
                .ForMember(biz => biz.JoinId, opt => opt.MapFrom(r => Converter.ToNullableInt32(r.Field<decimal?>("JoinId"))))
                .ForMember(biz => biz.PersistentContentId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("PersistentContentId"))))
                .ForMember(biz => biz.PersistentId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("PersistentId"))))
                .ForMember(biz => biz.RelateToPersistentContentId, opt => opt.MapFrom(r => Converter.ToNullableInt32(r.Field<decimal?>("RelateToPersistentContentId"))))
                .ForMember(biz => biz.Type, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("Type"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(r => r.Field<string>("Name")))
                .ForMember(biz => biz.PersistentName, opt => opt.MapFrom(r => r.Field<string>("PersistentName")));

            CreateMap<DataRow, VirtualFieldsRelation>(MemberList.None)
                .ForMember(biz => biz.BaseFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("BASE_ATTR_ID"))))
                .ForMember(biz => biz.BaseFieldContentId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("BASE_CNT_ID"))))
                .ForMember(biz => biz.VirtualFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("VIRTUAL_ATTR_ID"))))
                .ForMember(biz => biz.VirtualFieldContentId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("VIRTUAL_CNT_ID"))));

            CreateMap<DataRow, UnionFieldRelationCount>(MemberList.None)
                .ForMember(biz => biz.Count, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<int>("F_COUNT"))))
                .ForMember(biz => biz.UnionFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.Field<decimal>("UNION_FIELD_ID"))));


            CreateMap<UnionAttrDAL, UnionAttr>(MemberList.None)
                .ForMember(biz => biz.BaseFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.UnionFieldId)))
                .ForMember(biz => biz.VirtualFieldId, opt => opt.MapFrom(r => Converter.ToInt32(r.VirtualFieldId)))
                .ForMember(biz => biz.VirtualField, opt => opt.MapFrom(r => QPContext.Map<Field>(r.VirtualField)))
                .ForMember(biz => biz.BaseField, opt => opt.MapFrom(r => QPContext.Map<Field>(r.UnionField)));

            CreateMap<UnionAttr, UnionAttrDAL>(MemberList.None)
                .ForMember(data => data.UnionFieldId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.BaseFieldId)))
                .ForMember(data => data.VirtualFieldId, opt => opt.MapFrom(biz => Converter.ToDecimal(biz.VirtualFieldId)))
                .ForMember(data => data.UnionField, opt => opt.Ignore())
                .ForMember(data => data.VirtualField, opt => opt.Ignore());
        }
    }
}
