using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ObjectProfile : Profile
    {
        public ObjectProfile()
        {
            CreateMap<BllObject, ObjectDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.StatusType, opt => opt.Ignore())
                .ForMember(data => data.PageTemplate, opt => opt.Ignore())
                .ForMember(data => data.Page, opt => opt.Ignore())
                .ForMember(data => data.ObjectValues, opt => opt.Ignore())
                .ForMember(data => data.ObjectType, opt => opt.Ignore())
                .ForMember(data => data.DefaultFormat, opt => opt.Ignore())
                .ForMember(data => data.ChildObjectFormats, opt => opt.Ignore())
                .ForMember(data => data.InheritedObjects, opt => opt.Ignore())
                .ForMember(data => data.ObjectInheritedFrom, opt => opt.Ignore())
                .ForMember(data => data.Container, opt => opt.Ignore())
                .ForMember(data => data.ContentForm, opt => opt.Ignore())
                .ForMember(data => data.Locked, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (DateTime?)src.Locked))
                .ForMember(data => data.LockedBy, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (int?)src.LockedBy))
                .ForMember(data => data.LockedByUser, opt => opt.Ignore())
                .AfterMap(SetDalProperties);

            CreateMap<ObjectDAL, BllObject>(MemberList.None)
                .ForMember(data => data.ObjectInheritedFrom, opt => opt.Ignore())
                .ForMember(data => data.Container, opt => opt.Ignore())
                .ForMember(data => data.ContentForm, opt => opt.Ignore());

            CreateMap<ObjectValuesDAL, ObjectValue>(MemberList.None);
            CreateMap<ObjectValue, ObjectValuesDAL>(MemberList.None)
                .ForMember(data => data.Object, opt => opt.Ignore())
                ;

            CreateMap<DataRow, ObjectSearchListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.TemplateName, opt => opt.MapFrom(row => row.Field<string>("TemplateName")))
                .ForMember(biz => biz.PageName, opt => opt.MapFrom(row => row.Field<string>("PageName")))
                .ForMember(biz => biz.Description, opt => opt.Ignore())
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                .AfterMap(SetSearchBizProperties);

            CreateMap<DataRow, ObjectListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.TypeName, opt => opt.MapFrom(row => row.Field<string>("TypeName")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedBy"))))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                .ForMember(biz => biz.Icon, opt => opt.MapFrom(row => row.Field<string>("Icon")))
                .ForMember(biz => biz.LockedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal?>("LockedBy"), 0)))
                .ForMember(biz => biz.LockedByFullName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("LockedByFullName"), string.Empty)))
                .ForMember(biz => biz.ParentId, opt => opt.MapFrom(row => Converter.ToNullableInt32(row.Field<decimal?>("parentId"), null)))
                .ForMember(biz => biz.Overriden, opt => opt.MapFrom(row => row.Field<bool>("Overriden")))
                .AfterMap(SetListBizProperties);
        }

        private static void SetDalProperties(BllObject bizObject, ObjectDAL dataObject)
        {
            if (!bizObject.OverrideTemplateObject || !bizObject.IsNew || !bizObject.PageOrTemplate)
            {
                dataObject.ParentObjectId = null;
            }

            if (!(!bizObject.PageOrTemplate && (bizObject.IsCssType || bizObject.IsJavaScriptType))) // не объект шаблона - js / css
            {
                dataObject.Global = false;
            }
        }

        private static void SetListBizProperties(DataRow dataObject, ObjectListItem bizObject)
        {
            bizObject.LockedByIcon = LockableEntityObject.GetLockedByIcon(bizObject.LockedBy);
            bizObject.LockedByToolTip = LockableEntityObject.GetLockedByToolTip(bizObject.LockedBy, bizObject.LockedByFullName);
        }

        private static void SetSearchBizProperties(DataRow dataObject, ObjectSearchListItem bizObject)
        {
            bizObject.ParentId = dataObject.Field<decimal?>("PageId") == null ? Converter.ToInt32(dataObject.Field<decimal?>("PageTemplateId")) : Converter.ToInt32(dataObject.Field<decimal?>("PageId"));
        }

    }
}
