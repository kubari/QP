using System.Data;
using AutoMapper;
using Quantumart.QP8.BLL.Services.DTO;
using Quantumart.QP8.Resources;

namespace Quantumart.QP8.BLL.MapperProfiles
{
    internal class ActionPermissionTreeProfile : Profile
    {
        public ActionPermissionTreeProfile()
        {
            CreateMap<DataRow, ActionPermissionTreeNode>(MemberList.None)
                .ForMember(biz => biz.Id, opt => opt.MapFrom(r => r.Field<int>("ID")))
                .ForMember(biz => biz.Text, opt => opt.MapFrom(r => FormatText(r)))
            ;
        }

        private string FormatText(DataRow row)
        {
            var name = Translator.Translate(row.Field<string>("NAME"));
            var levelName = Translator.Translate(row.Field<string>("PERMISSION_LEVEL_NAME"));
            var isExplicit = row.Field<bool>("IsExplicit");
            if (string.IsNullOrWhiteSpace(levelName))
            {
                return $"{name} – {EntityPermissionStrings.UndefinedPermissionLevel}";
            }

            return $"{name} – {levelName} ({(isExplicit ? EntityPermissionStrings.Explicit : EntityPermissionStrings.Implicit)})";
        }
    }
}
