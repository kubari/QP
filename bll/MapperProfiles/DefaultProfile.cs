using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.Models.NotificationSender;
using Quantumart.QP8.BLL.Models.XmlDbUpdate;
using Quantumart.QP8.BLL.Repository;
using Quantumart.QP8.BLL.Repository.ArticleMatching.Models;
using Quantumart.QP8.BLL.Services;
using Quantumart.QP8.BLL.Services.API.Models;
using Quantumart.QP8.BLL.Services.VisualEditor;
using Quantumart.QP8.DAL.DTO;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            CreateMap<decimal, bool>().ConvertUsing(src => Converter.ToBoolean(src));
            CreateMap<decimal?, int?>().ConvertUsing(src => Converter.ToNullableInt32(src));
            CreateMap<bool, decimal>().ConvertUsing(src => Converter.ToDecimal(src));
            CreateMap<int, decimal>().ConvertUsing(src => src);
            CreateMap<decimal, int>().ConvertUsing(src => Converter.ToInt32(src));
            CreateMap<int?, decimal?>().ConvertUsing(src => src);

            CreateMap<DataRow, SearchInArticlesResultItem>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<SearchInArticlesResultItem>(src));
            CreateMap<DataRow, VisualEditFieldParams>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<VisualEditFieldParams>(src));
            CreateMap<DataRow, BackendActionCacheRecord>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<BackendActionCacheRecord>(src));
            CreateMap<DataRow, SchemaInfo>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<SchemaInfo>(src));
            CreateMap<DataRow, RelationData>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<RelationData>(src));
            CreateMap<DataRow, InsertData>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<InsertData>(src));
            CreateMap<DataRow, ArticleInfo>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<ArticleInfo>(src));
            CreateMap<DataRow, PageInfo>(MemberList.None)
                .ConvertUsing(src => Converter.ToModelFromDataRow<PageInfo>(src));

            CreateMap<XmlDbUpdateActionsLogModel, XmlDbUpdateActionsLogEntity>(MemberList.None);
            CreateMap<XmlDbUpdateLogModel, XmlDbUpdateLogEntity>(MemberList.None);

            CreateMap<CustomFilter, CustomFilterItem>(MemberList.None);

            CreateMap<ExternalNotificationDAL, ExternalNotificationModel>(MemberList.None);
            CreateMap<ExternalNotificationModel, ExternalNotificationDAL>(MemberList.None);

            CreateMap<SessionsLogDAL, SessionsLog>(MemberList.None);
            CreateMap<SessionsLog, SessionsLogDAL>(MemberList.None);

            CreateMap<FieldTypeDAL, FieldType>(MemberList.None);
            CreateMap<FieldType, FieldTypeDAL>(MemberList.None);

            CreateMap<ArticleVersionDAL, ArticleVersion>(MemberList.None);
            CreateMap<ArticleVersion, ArticleVersionDAL>(MemberList.None);

            CreateMap<ActionViewDAL, BackendActionView>(MemberList.None);
            CreateMap<BackendActionView, ActionViewDAL>(MemberList.None);

            CreateMap<ContentConstraintDAL, ContentConstraint>(MemberList.None);
            CreateMap<ContentConstraint, ContentConstraintDAL>(MemberList.None);

            CreateMap<MaskTemplateDAL, MaskTemplate>(MemberList.None);
            CreateMap<MaskTemplate, MaskTemplateDAL>(MemberList.None);

            CreateMap<BackendActionLogDAL, BackendActionLog>(MemberList.None);
            CreateMap<BackendActionLog, BackendActionLogDAL>(MemberList.None);

            CreateMap<NetLanguagesDAL, NetLanguage>(MemberList.None);
            CreateMap<NetLanguage, NetLanguagesDAL>(MemberList.None);

            CreateMap<LocaleDAL, Locale>(MemberList.None);
            CreateMap<Locale, LocaleDAL>(MemberList.None);

            CreateMap<CharsetDAL, Charset>(MemberList.None);
            CreateMap<Charset, CharsetDAL>(MemberList.None);

            CreateMap<ObjectTypeDAL, ObjectType>(MemberList.None);
            CreateMap<ObjectType, ObjectTypeDAL>(MemberList.None);

            CreateMap<ObjectFormatVersionDAL, ObjectFormatVersion>(MemberList.None);
            CreateMap<ObjectFormatVersion, ObjectFormatVersionDAL>(MemberList.None);

            CreateMap<DbDAL, Db>(MemberList.None);
            CreateMap<Db, DbDAL>(MemberList.None);

            CreateMap<BackendActionLogUserGroupDAL, BackendActionLogUserGroup>(MemberList.None);
            CreateMap<BackendActionLogUserGroup, BackendActionLogUserGroupDAL>(MemberList.None);

            CreateMap<ViewTypeDAL, ViewType>(MemberList.None)
                .ForMember(biz => biz.Name, opt => opt.MapFrom(data => Translator.Translate(data.Name)));

            CreateMap<SystemNotificationModel, SystemNotificationDAL>(MemberList.None);
            CreateMap<SystemNotificationDAL, SystemNotificationModel>(MemberList.None);
        }
    }
}
