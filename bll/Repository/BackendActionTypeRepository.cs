using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class BackendActionTypeRepository
    {
        private static IQueryable<ActionTypeDAL> DefaultActionTypeQuery => QPContext.EFContext.ActionTypeSet
            .Include("PermissionLevel");

        internal static IEnumerable<BackendActionType> GetList()
        {
            var result = QPContext.Map<List<BackendActionType>>(DefaultActionTypeQuery.ToList());
            result.ForEach(n =>
            {
                n.Name = n.NotTranslatedName;
            });
            return result;
        }

        /// <summary>
        /// Возвращает тип действия по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns>тип действия</returns>
        internal static BackendActionType GetById(int id)
        {
            var actionType = QPContext.Map<BackendActionType>(DefaultActionTypeQuery.Single(r => r.Id == id));
            return actionType;
        }

        /// <summary>
        /// Возвращает тип действия по его коду
        /// </summary>
        /// <param name="code">код типа действия</param>
        /// <returns>тип действия</returns>
        internal static BackendActionType GetByCode(string code)
        {
            var actionType = QPContext.Map<BackendActionType>(DefaultActionTypeQuery.Single(r => r.Code == code));
            return actionType;
        }

        /// <summary>
        /// Возвращает код типа действия по коду действия
        /// </summary>
        /// <param name="actionCode">код действия</param>
        /// <returns>код типа действия</returns>
        internal static string GetCodeByActionCode(string actionCode)
        {
            return QPContext.EFContext.BackendActionSet.Include("ActionType")
                .Single(a => a.Code == actionCode)
                .ActionType.Code;

            //GetActionTypeCodeByActionCode(actionCode);
        }
    }
}
