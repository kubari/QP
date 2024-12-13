using System.Linq;

namespace Quantumart.QP8.BLL.Repository
{
    internal class ObjectTypeRepository
    {
        public static ObjectType GetByName(string name)
        {
            var entities = QPContext.EFContext;
            return QPContext.Map<ObjectType>(entities.ObjectTypeSet.SingleOrDefault(x => x.Name == name));
        }
    }
}
