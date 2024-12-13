using System;
using System.Data;
using System.Linq;
using AutoMapper;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class BackendActionProfile : Profile
    {
        public BackendActionProfile()
        {

            CreateMap<BackendActionDAL, BackendAction>(MemberList.None)
                .ForMember(biz => biz.NextFailedActionCode, opt => opt.MapFrom(data => data.NextFailedAction != null ? data.NextFailedAction.Code : null))
                .ForMember(biz => biz.NextSuccessfulActionCode, opt => opt.MapFrom(data => data.NextSuccessfulAction != null ? data.NextSuccessfulAction.Code : null))
                .ForMember(biz => biz.ExcludeCodes, opt => opt.MapFrom(data => data.ExcludedByBinds != null ? data.ExcludedByBinds.Select(n => n.Excludes.Code).ToArray() : null))
                .ForMember(biz => biz.Name, opt => opt.MapFrom(data => Translator.Translate(data.Name)))
                .ForMember(biz => biz.NotTranslatedName, opt => opt.MapFrom(data => Translator.Translate(data.Name)))
                .ForMember(biz => biz.ShortName, opt => opt.MapFrom(data => Translator.Translate(data.ShortName)))
                .ForMember(biz => biz.ConfirmPhrase, opt => opt.MapFrom(data => Translator.Translate(data.ConfirmPhrase)))
                .ForMember(biz => biz.TabId, opt => opt.MapFrom(data => Converter.ToNullableInt32(data.TabId)));

            CreateMap<DataRow, BackendActionLog>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(row => row.Field<int>("Id")))
                .ForMember(biz => biz.ExecutionTime, opt => opt.MapFrom(row => row.Field<DateTime>("ExecutionTime")))
                .ForMember(biz => biz.ActionName, opt => opt.MapFrom(row => row.Field<string>("ActionName")))
                .ForMember(biz => biz.ActionTypeCode, opt => opt.MapFrom(row => row.Field<string>("ActionTypeCode")))
                .ForMember(biz => biz.ActionTypeName, opt => opt.MapFrom(row => row.Field<string>("ActionTypeName")))
                .ForMember(biz => biz.EntityTypeCode, opt => opt.MapFrom(row => row.Field<string>("EntityTypeCode")))
                .ForMember(biz => biz.EntityTypeName, opt => opt.MapFrom(row => row.Field<string>("EntityTypeName")))
                .ForMember(biz => biz.EntityStringId, opt => opt.MapFrom(row => row.Field<string>("EntityStringId")))
                .ForMember(biz => biz.ParentEntityId, opt => opt.MapFrom(row => Converter.ToNullableInt32(row.Field<decimal?>("ParentEntityId"))))
                .ForMember(biz => biz.EntityTitle, opt => opt.MapFrom(row => row.Field<string>("EntityTitle")))
                .ForMember(biz => biz.UserId, opt => opt.MapFrom(row => Converter.ToNullableInt32(row.Field<decimal?>("UserId"))))
                .ForMember(biz => biz.IsApi, opt => opt.MapFrom(row => row.Field<bool>("IsApi")))
                .ForMember(biz => biz.UserLogin, opt => opt.MapFrom(row => row.Field<string>("UserLogin")))
                .ForMember(biz => biz.UserIp, opt => opt.MapFrom(row => row.Field<string>("UserIp")));

            CreateMap<BackendAction, BackendActionDAL>(MemberList.None)
                .ForMember(data => data.ActionType, opt => opt.Ignore())
                .ForMember(data => data.EntityType, opt => opt.Ignore())
                .ForMember(data => data.CustomActions, opt => opt.Ignore())
                .ForMember(data => data.ToolbarButtons, opt => opt.Ignore())
                .ForMember(data => data.ContextMenuItems, opt => opt.Ignore())
                .ForMember(data => data.DefaultViewType, opt => opt.Ignore())
                .ForMember(data => data.NextFailedAction, opt => opt.Ignore())
                .ForMember(data => data.NextSuccessfulAction, opt => opt.Ignore())
                .ForMember(data => data.ParentAction, opt => opt.Ignore())
                .ForMember(data => data.ParentPreFailedActions, opt => opt.Ignore())
                .ForMember(data => data.ParentPreSuccessfulActions, opt => opt.Ignore())
                .ForMember(data => data.Views, opt => opt.Ignore());

            CreateMap<DataRow, BackendActionStatus>(MemberList.None)
                .ForMember(biz => biz.Code, opt => opt.MapFrom(row => row.Field<string>("CODE")))
                .ForMember(biz => biz.Visible, opt => opt.MapFrom(row => row.Field<bool>(FieldName.Visible)));

            CreateMap<ActionTypeDAL, BackendActionType>(MemberList.None)
                .ForMember(biz => biz.Name, opt => opt.MapFrom(data => Translator.Translate(data.Name)))
                .ForMember(biz => biz.NotTranslatedName, opt => opt.MapFrom(data => data.Name))
                .ForMember(biz => biz.RequiredPermissionLevel, opt => opt.MapFrom(data => Converter.ToInt32(data.PermissionLevel.Level)));

        }
    }
}
