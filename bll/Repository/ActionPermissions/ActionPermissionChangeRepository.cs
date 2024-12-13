using System.Linq;

namespace Quantumart.QP8.BLL.Repository.ActionPermissions
{
    internal class ActionPermissionChangeRepository : IActionPermissionChangeRepository
    {
        public EntityPermission ReadForUser(int parentId, int userId)
        {
            var permission = QPContext.EFContext.BackendActionPermissionSet.SingleOrDefault(p => p.ActionId == parentId && p.UserId == userId);
            return QPContext.Map<EntityPermission>(permission);
        }

        public EntityPermission ReadForGroup(int parentId, int groupId)
        {
            var permission = QPContext.EFContext.BackendActionPermissionSet.SingleOrDefault(p => p.ActionId == parentId && p.GroupId == groupId);
            return QPContext.Map<EntityPermission>(permission);
        }
    }
}
