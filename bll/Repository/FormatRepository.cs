using System.Collections.Generic;
using System.Linq;
using Quantumart.QP8.DAL.Entities;
using Quantumart.QP8.Utils;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class FormatRepository
    {
        internal static ObjectFormat SaveObjectFormatProperties(ObjectFormat objectFormat) => DefaultRepository.Save<ObjectFormat, ObjectFormatDAL>(objectFormat);

        internal static ObjectFormat UpdateObjectFormatProperties(ObjectFormat objectFormat) => DefaultRepository.Update<ObjectFormat, ObjectFormatDAL>(objectFormat);

        internal static IEnumerable<EntityObject> GetList(IEnumerable<int> ids, bool pageOrTemplate)
        {
            IEnumerable<decimal> decIDs = Converter.ToDecimalCollection(ids).Distinct().ToArray();
            var result = QPContext.Map<List<ObjectFormat>>(
                QPContext.EFContext.ObjectFormatSet
                    .Where(f => decIDs.Contains(f.Id))
                    .ToList()
                );

            foreach (var item in result)
            {
                item.PageOrTemplate = pageOrTemplate;
            }

            return result;
        }
    }
}
