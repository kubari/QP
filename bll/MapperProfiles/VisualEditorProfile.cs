using System;
using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.BLL.Services.VisualEditor;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class VisualEditorProfile : Profile
    {
        public VisualEditorProfile()
        {
            CreateMap<VeStyleDAL, VisualEditorStyle>(MemberList.None).AfterMap((dataObject, bizObject) => bizObject.Init());
            CreateMap<VisualEditorStyle, VeStyleDAL>(MemberList.None).ForMember(data => data.LastModifiedByUser, opt => opt.Ignore());

            CreateMap<VePluginDAL, VisualEditorPlugin>(MemberList.None);
            CreateMap<VisualEditorPlugin, VePluginDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.VeCommands, opt => opt.Ignore());

            CreateMap<VeCommandDAL, VisualEditorCommand>(MemberList.None).ForMember(biz => biz.Alias, opt => opt.MapFrom(data => Translator.Translate(data.Alias)));
            CreateMap<VisualEditorCommand, VeCommandDAL>(MemberList.None).ForMember(data => data.LastModifiedByUser, opt => opt.Ignore()).ForMember(data => data.VePlugin, opt => opt.Ignore());

            CreateMap<DataRow, VisualEditorStyle>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("DESCRIPTION")))
                .ForMember(biz => biz.Tag, opt => opt.MapFrom(row => row.Field<string>("TAG")))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => row.Field<int>("ORDER")))
                .ForMember(biz => biz.OverridesTag, opt => opt.MapFrom(row => row.Field<string>("OVERRIDES_TAG")))
                .ForMember(biz => biz.IsFormat, opt => opt.MapFrom(row => row.Field<bool>("IS_FORMAT")))
                .ForMember(biz => biz.IsSystem, opt => opt.MapFrom(row => row.Field<bool>("IS_SYSTEM")))
                .ForMember(biz => biz.Attributes, opt => opt.MapFrom(row => row.Field<string>("ATTRIBUTES")))
                .ForMember(biz => biz.Styles, opt => opt.MapFrom(row => row.Field<string>("STYLES")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.On, opt => opt.MapFrom(row => row.Field<bool>("ON")));

            CreateMap<DataRow, VisualEditorStyleListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Tag, opt => opt.MapFrom(row => row.Field<string>("Tag")))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<int>("Order"))))
                .ForMember(biz => biz.IsFormat, opt => opt.MapFrom(row => row.Field<bool>("IsFormat")))
                .ForMember(biz => biz.IsSystem, opt => opt.MapFrom(row => row.Field<bool>("IsSystem")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")));

            CreateMap<DataRow, VisualEditorPluginListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.Url, opt => opt.MapFrom(row => row.Field<string>("Url")))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<int>("Order"))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedBy"))))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")));

            CreateMap<DataRow, VisualEditorCommand>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("ID"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("NAME")))
                .ForMember(biz => biz.Alias, opt => opt.MapFrom(row => Translator.Translate(row.Field<string>("ALIAS"))))
                .ForMember(biz => biz.RowOrder, opt => opt.MapFrom(row => row.Field<int>("ROW_ORDER")))
                .ForMember(biz => biz.ToolbarInRowOrder, opt => opt.MapFrom(row => row.Field<int>("TOOLBAR_IN_ROW_ORDER")))
                .ForMember(biz => biz.GroupInToolbarOrder, opt => opt.MapFrom(row => row.Field<int>("GROUP_IN_TOOLBAR_ORDER")))
                .ForMember(biz => biz.CommandInGroupOrder, opt => opt.MapFrom(row => row.Field<int>("COMMAND_IN_GROUP_ORDER")))
                .ForMember(biz => biz.On, opt => opt.MapFrom(row => row.Field<bool>("ON")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Created)))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>(FieldName.Modified)))
                .ForMember(biz => biz.PluginId, opt => opt.MapFrom(row => Converter.ToNullableInt32(row.Field<decimal?>("PLUGIN_ID"))));

        }
    }
}
