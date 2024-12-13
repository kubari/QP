using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class SiteProfile : Profile
    {
        public SiteProfile()
        {
            CreateMap<SiteDAL, Site>(MemberList.None)
                .ForMember(biz => biz.LockedBy, opt => opt.MapFrom(src => Converter.ToInt32(src.LockedBy)))
                .ForMember(biz => biz.Locked, opt => opt.MapFrom(src => Converter.ToDateTime(src.Locked)))
                .ForMember(biz => biz.IsLive, opt => opt.MapFrom(src => Converter.ToBoolean(src.IsLive)))
                .ForMember(biz => biz.AssemblingType, opt => opt.MapFrom(src => src.ScriptLanguage))
                .ForMember(biz => biz.SeparateDns, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.StageDns)))
                .ForMember(biz => biz.ExternalCss, opt => opt.MapFrom(src => src.ExternalCss));

            CreateMap<Site, SiteDAL>(MemberList.None)
                .ForMember(data => data.IsLive, opt => opt.MapFrom(src => Converter.ToInt32(src.IsLive)))
                .ForMember(data => data.ScriptLanguage, opt => opt.MapFrom(src => src.AssemblingType))
                .ForMember(data => data.Locked, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (DateTime?)src.Locked))
                .ForMember(data => data.LockedBy, opt => opt.MapFrom(src => src.LockedBy == 0 ? null : (int?)src.LockedBy))
                .ForMember(data => data.AccessRules, opt => opt.Ignore())
                .ForMember(data => data.CodeSnippets, opt => opt.Ignore())
                .ForMember(data => data.ContentGroups, opt => opt.Ignore())
                .ForMember(data => data.Folders, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.LockedByUser, opt => opt.Ignore())
                .ForMember(data => data.PageTemplates, opt => opt.Ignore())
                .ForMember(data => data.Statuses, opt => opt.Ignore())
                .ForMember(data => data.Styles, opt => opt.Ignore())
                .ForMember(data => data.Workflows, opt => opt.Ignore())
                .ForMember(data => data.CustomActions, opt => opt.Ignore());

            CreateMap<DataRow, SiteListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("Description"), string.Empty)))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByUser")))
                .ForMember(biz => biz.LockedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal?>("LockedBy"), 0)))
                .ForMember(biz => biz.LockedByFullName, opt => opt.MapFrom(row => Converter.ToString(row.Field<string>("LockedByFullName"), string.Empty)))
                .ForMember(biz => biz.IsLive, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<string>("IsLive"))))
                .ForMember(biz => biz.Dns, opt => opt.MapFrom(row => row.Field<string>("Dns")))
                .ForMember(biz => biz.UploadUrl, opt => opt.MapFrom(row => row.Field<string>("UploadUrl")))
                .AfterMap(SetBizProperties);
        }

        private static void SetBizProperties(DataRow dataObject, SiteListItem bizObject)
        {
            bizObject.LockedByIcon = LockableEntityObject.GetLockedByIcon(bizObject.LockedBy);
            bizObject.LockedByToolTip = LockableEntityObject.GetLockedByToolTip(bizObject.LockedBy, bizObject.LockedByFullName);
        }
    }
}
