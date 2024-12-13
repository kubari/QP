using System.Collections.Generic;
using System.Linq;

namespace Quantumart.QP8.BLL.Repository.EntityPermissions
{
    internal static class CommonPermissionRepository
    {
        internal static IEnumerable<EntityPermissionLevel> GetPermissionLevels()
        {
            return QPContext.Map<EntityPermissionLevel[]>(
                QPContext.EFContext.PermissionLevelSet.OrderByDescending(p => p.Level).ToList()
            );
        }
    }
}
