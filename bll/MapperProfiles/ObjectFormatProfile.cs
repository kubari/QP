using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.BLL.Services.DTO;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ObjectFormatProfile : Profile
    {
        public ObjectFormatProfile()
        {
            CreateMap<ObjectFormat, ObjectFormatDAL>(MemberList.None)
                .ForMember(data => data.NetLanguages, opt => opt.Ignore())
                .ForMember(data => data.Notifications, opt => opt.Ignore())
                .ForMember(data => data.Object, opt => opt.Ignore())
                .ForMember(data => data.Object1, opt => opt.Ignore())
                .ForMember(data => data.ObjectFormatVersion, opt => opt.Ignore())
                .ForMember(data => data.PageTraceFormat, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Locked, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (DateTime?)src.Locked))
                .ForMember(data => data.LockedBy, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (int?)src.LockedBy))
                .ForMember(data => data.LockedByUser, opt => opt.Ignore())
                .AfterMap(SetDalProperties);

            CreateMap<ObjectFormatDAL, ObjectFormat>(MemberList.None);

            CreateMap<DataRow, ObjectFormatListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedBy"))))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                .ForMember(biz => biz.LockedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal?>("LockedBy"), 0)))
                .ForMember(biz => biz.LockedByFullName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("LockedByFullName"), string.Empty)))
                .AfterMap(SetRowBizProperties);

            CreateMap<DataRow, ObjectFormatSearchResultListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.TemplateName, opt => opt.MapFrom(row => row.Field<string>("TemplateName")))
                .ForMember(biz => biz.PageName, opt => opt.MapFrom(row => row.Field<string>("PageName")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                .ForMember(biz => biz.ParentName, opt => opt.MapFrom(row => row.Field<string>("ParentName")))
                .ForMember(biz => biz.ParentId, opt => opt.MapFrom(row => row.Field<decimal>("ParentId")));

            CreateMap<DataRow, ObjectFormatVersionListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.ModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")))
                ;

            CreateMap<DataRow, TemplateObjectFormatDto>(MemberList.None)
                .ForMember(biz => biz.TemplateName, opt => opt.MapFrom(row => row.Field<string>("TemplateName")))
                .ForMember(biz => biz.ObjectName, opt => opt.MapFrom(row => row.Field<string>("ObjectName")))
                .ForMember(biz => biz.FormatName, opt => opt.MapFrom(row => row.Field<string>("FormatName")))
                ;
        }

        private static void SetDalProperties(ObjectFormat bizObject, ObjectFormatDAL dataObject)
        {
            if (!bizObject.Assembled.HasValue)
            {
                using (new QPConnectionScope())
                {
                    dataObject.Assembled = Common.GetSqlDate(QPConnectionScope.Current.DbConnection);
                }
            }
        }

        private static void SetRowBizProperties(DataRow dataObject, ObjectFormatListItem bizObject)
        {
            bizObject.IsDefault = Converter.ToInt32(dataObject.Field<decimal?>("FormatIdToCheck"), 0) == bizObject.Id;
            bizObject.LockedByIcon = LockableEntityObject.GetLockedByIcon(bizObject.LockedBy);
            bizObject.LockedByToolTip = LockableEntityObject.GetLockedByToolTip(bizObject.LockedBy, bizObject.LockedByFullName);
        }
    }
}
