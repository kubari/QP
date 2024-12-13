using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.Constants;
using Quantumart.QP8.Utils;
using SiteFolderDAL = Quantumart.QP8.DAL.Entities.SiteFolderDAL;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class SiteFolderProfile : Profile
    {
        public SiteFolderProfile()
        {
            CreateMap<SiteFolderDAL, SiteFolder>(MemberList.None)
                .ForMember(biz => biz.StoredPath, opt => opt.MapFrom(data => data.Path));

            CreateMap<SiteFolder, SiteFolderDAL>(MemberList.None)
                .ForMember(data => data.Site, opt => opt.Ignore())
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore());

            CreateMap<DataRow, SiteFolder>(MemberList.None).BeforeMap(SetBeforeBizProperties)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("FOLDER_ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>(FieldName.LastModifiedBy))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.HasChildren, opt => opt.MapFrom(row => row.Field<bool>("HAS_CHILDREN")))
                .ForMember(biz => biz.LastModifiedByUser, opt => opt.MapFrom(row => GetModifierUserFromRow(row)));
        }

        public static User GetModifierUserFromRow(DataRow row) =>
            new User()
            {
                Id = Converter.ToInt32(row.Field<decimal>("MODIFIER_USER_ID")),
                FirstName = row.Field<string>("MODIFIER_FIRST_NAME"),
                LastName = row.Field<string>("MODIFIER_LAST_NAME"),
                Email = row.Field<string>("MODIFIER_EMAIL"),
                LogOn = row.Field<string>(FieldName.ModifierLogin)
            };

        private static void SetBeforeBizProperties(DataRow dataObject, SiteFolder bizObject)
        {
            bizObject.AutoLoadChildren = false;
        }

    }
}
