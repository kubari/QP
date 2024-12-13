using System;
using System.Data;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using QP8.Plugins.Contract;
using Quantumart.QP8.BLL.ListItems;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    public class QpPluginProfile : Profile
    {
        public static T Read<T>(string data)
        {
            var reader = new JTokenReader(new JValue(data));
            reader.Read();
            var result = (T)new StringEnumConverter().ReadJson(reader, typeof(T), null, null);
            return result;
        }

        public static string Write(object data)
        {
            var arr = new JArray();
            var writer = new JTokenWriter(arr);
            new StringEnumConverter().WriteJson(writer, data, null);
            var result = arr.First?.ToString();
            return result;
        }

        public QpPluginProfile()
        {
            CreateMap<DataRow, QpPluginVersionListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")));

            CreateMap<DataRow, QpPluginListItem>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("Id"))))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(row => row.Field<string>("Name")))
                .ForMember(biz => biz.Description, opt => opt.MapFrom(row => row.Field<string>("Description")))
                .ForMember(biz => biz.ServiceUrl, opt => opt.MapFrom(row => row.Field<string>("ServiceUrl")))
                .ForMember(biz => biz.Order, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<int>("Order"))))
                .ForMember(biz => biz.Created, opt => opt.MapFrom(row => row.Field<DateTime>("Created")))
                .ForMember(biz => biz.Modified, opt => opt.MapFrom(row => row.Field<DateTime>("Modified")))
                .ForMember(biz => biz.LastModifiedBy, opt => opt.MapFrom(row => Converter.ToInt32(row.Field<decimal>("LastModifiedBy"))))
                .ForMember(biz => biz.LastModifiedByLogin, opt => opt.MapFrom(row => row.Field<string>("LastModifiedByLogin")));

            CreateMap<PluginDAL, QpPlugin>(MemberList.None)
                .ForMember(biz => biz.OldContract, opt => opt.MapFrom(n => n.Contract))
                .ForMember(biz => biz.OldLastModifiedBy, opt => opt.MapFrom(n => n.LastModifiedBy))
                .ForMember(biz => biz.OldModified, opt => opt.MapFrom(n => n.Modified))
                ;

            CreateMap<PluginFieldDAL, QpPluginField>(MemberList.None)
                .ForMember(biz => biz.RelationType, opt => opt.MapFrom(data => Read<QpPluginRelationType>(data.RelationType)))
                .ForMember(biz => biz.ValueType, opt => opt.MapFrom(data => Read<QpPluginValueType>(data.ValueType)))
                ;

            CreateMap<PluginVersionDAL, QpPluginVersion>(MemberList.None);

            CreateMap<QpPlugin, PluginDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                ;

            CreateMap<QpPluginField, PluginFieldDAL>(MemberList.None)
                .ForMember(data => data.RelationType, opt => opt.MapFrom(biz => Write(biz.RelationType)))
                .ForMember(data => data.ValueType, opt => opt.MapFrom(biz => Write(biz.ValueType)))
                ;

            CreateMap<QpPluginVersion, PluginVersionDAL>(MemberList.None)
                .ForMember(data => data.LastModifiedByUser, opt => opt.Ignore())
                .ForMember(data => data.Plugin, opt => opt.Ignore())
                ;

        }
    }
}
