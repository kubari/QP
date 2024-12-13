using System.Collections.Generic;
using System.Linq;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class MaskTemplateRepository
    {
        /// <summary>
        /// Получить все шаблоны масок
        /// </summary>
        public static IEnumerable<MaskTemplate> GetAllMaskTemplates()
        {
            return QPContext.Map<MaskTemplate[]>(
                QPContext.EFContext.MaskTemplateSet.ToList()
            );
        }
    }
}
