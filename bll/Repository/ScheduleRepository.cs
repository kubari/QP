using System.Collections.Generic;
using System.Linq;
using Quantumart.QP8.Constants;
using Quantumart.QP8.DAL;
using Quantumart.QP8.DAL.Entities;

namespace Quantumart.QP8.BLL.Repository
{
    internal static class ScheduleRepository
    {
        internal static ArticleSchedule GetSchedule(Article item)
        {
            var dal = GetDalByArticleId(item.Id);
            return dal == null ?
                ArticleSchedule.CreateSchedule(item) :
                QPContext.Map<ArticleSchedule>(dal);
        }

        internal static ArticleSchedule GetScheduleById(int id)
        {
            var dal = GetDalById(id);
            return dal == null ? null : QPContext.Map<ArticleSchedule>(dal);
        }

        internal static void UpdateSchedule(Article article)
        {
            var item = article.Schedule;
            if (item != null)
            {
                item.Article = article;
                item.ArticleId = article.Id;

                var originalId = item.Id;
                item.Id = 0;

                var dalItem = QPContext.Map<ArticleScheduleDAL>(item);
                var itemPersisted = originalId != 0;
                var hasChanges = !itemPersisted;

                if (itemPersisted)
                {
                    var originalItem = GetDalById(originalId);
                    hasChanges = DetectChanges(originalItem, dalItem);
                    if (hasChanges)
                    {
                        DefaultRepository.Delete<ArticleScheduleDAL>(originalId);
                    }

                }

                var needToPersist = dalItem.FreqType != ScheduleFreqTypes.None && hasChanges;
                if (needToPersist)
                {
                    dalItem.UseService = true;
                    dalItem.Created = article.Modified;
                    dalItem.Modified = article.Modified;
                    dalItem.LastModifiedBy = article.LastModifiedBy;
                    DefaultRepository.SimpleSave(dalItem);
                }
            }
        }

        private static bool DetectChanges(ArticleScheduleDAL originalItem, ArticleScheduleDAL dalItem) => originalItem.EndDate != dalItem.EndDate ||
            originalItem.StartDate != dalItem.StartDate ||
            originalItem.Duration != dalItem.Duration ||
            originalItem.DurationUnits != dalItem.DurationUnits ||
            originalItem.UseDuration != dalItem.UseDuration ||
            originalItem.Occurences != dalItem.Occurences ||
            originalItem.MaximumOccurences != dalItem.MaximumOccurences ||
            originalItem.FreqType != dalItem.FreqType ||
            originalItem.FreqSubdayType != dalItem.FreqSubdayType ||
            originalItem.FreqSubdayInterval != dalItem.FreqSubdayInterval ||
            originalItem.FreqRelativeInterval != dalItem.FreqRelativeInterval ||
            originalItem.FreqRecurrenceFactor != dalItem.FreqRecurrenceFactor ||
            originalItem.FreqInterval != dalItem.FreqInterval;

        private static ArticleScheduleDAL GetDalByArticleId(int id)
        {
            return QPContext.EFContext.ArticleScheduleSet.SingleOrDefault(n => n.ArticleId == id);
        }

        private static ArticleScheduleDAL GetDalById(int id)
        {
            return QPContext.EFContext.ArticleScheduleSet.SingleOrDefault(n => n.Id == id);
        }

        private static void Delete(int id)
        {
            DefaultRepository.Delete<ArticleScheduleDAL>(id);
        }

        internal static void Delete(ArticleSchedule schedule)
        {
            if (schedule != null)
            {
                Delete(schedule.Id);
            }
        }

        public static IEnumerable<ArticleScheduleTask> GetScheduleTaskList()
        {
            return QPContext.Map<ArticleScheduleTask[]>(
                QPContext.EFContext.ArticleScheduleSet.Where(t => !t.Deactivate && t.UseService).ToList()
                );
        }

        internal static void CopyScheduleToChildDelays(Article item)
        {
            using var scope = new QPConnectionScope();
            Common.UpdateChildDelayedSchedule(scope.DbConnection, item.Id);
        }
    }
}
