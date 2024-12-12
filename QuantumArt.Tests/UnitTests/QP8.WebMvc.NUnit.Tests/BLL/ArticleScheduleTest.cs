using System;
using NUnit.Framework;
using Quantumart.QP8.BLL;
using C = Quantumart.QP8.Constants;

namespace QP8.WebMvc.NUnit.Tests.BLL
{
    [TestFixture]
    public class ArticleScheduleTest
    {
        [Test]
        public void CopyFromTest()
        {
            var srcArticle = new Article();
            var source = new ArticleSchedule
            {
                Article = srcArticle,
                ArticleId = 10,
                Id = 100,

                EndDate = DateTime.Now.AddDays(-1),
                PublicationDate = DateTime.Now.AddDays(-2),
                ScheduleType = C.ScheduleTypeEnum.Recurring,
                StartDate = DateTime.Now.AddDays(-3),
                StartRightNow = true,
                WithoutEndDate = true,

                Recurring = new RecurringSchedule
                {
                    DayOfMonth = 1,
                    DayOfWeek = C.DayOfWeek.Wednesday,
                    DaySpecifyingType = C.DaySpecifyingType.DayOfWeek,
                    DurationUnit = C.ShowDurationUnit.Months,
                    DurationValue = 2,
                    Month = 3,
                    OnMonday = true,
                    RepetitionEndDate = DateTime.Now.AddDays(1),
                    RepetitionNoEnd = true,
                    RepetitionStartDate = DateTime.Now.AddDays(2),
                    ScheduleRecurringType = C.ScheduleRecurringType.Monthly,
                    ScheduleRecurringValue = 3,
                    ShowEndTime = DateTime.Now.Date + TimeSpan.FromMinutes(22),
                    ShowLimitationType = C.ShowLimitationType.Duration,
                    ShowStartTime = DateTime.Now.Date + TimeSpan.FromMinutes(122),
                    WeekOfMonth = C.WeekOfMonth.LastWeek
                }
            };

            var dest = new ArticleSchedule
            {
                Article = new Article(),
                ArticleId = 20,
                Id = 200,
                Recurring = RecurringSchedule.Empty
            };

            dest.CopyFrom(source);

            Assert.That(dest.ArticleId, Is.EqualTo(20));
            Assert.That(dest.Id, Is.EqualTo(200));

            Assert.That(source.EndDate, Is.EqualTo(dest.EndDate));
            Assert.That(source.IsVisible, Is.EqualTo(dest.IsVisible));
            Assert.That(source.PublicationDate, Is.EqualTo(dest.PublicationDate));
            Assert.That(source.ScheduleType, Is.EqualTo(dest.ScheduleType));
            Assert.That(source.StartDate, Is.EqualTo(dest.StartDate));
            Assert.That(source.StartRightNow, Is.EqualTo(dest.StartRightNow));
            Assert.That(source.WithoutEndDate, Is.EqualTo(dest.WithoutEndDate));

            Assert.That(source.Recurring.DayOfMonth, Is.EqualTo(dest.Recurring.DayOfMonth));
            Assert.That(source.Recurring.DayOfWeek, Is.EqualTo(dest.Recurring.DayOfWeek));
            Assert.That(source.Recurring.DaySpecifyingType, Is.EqualTo(dest.Recurring.DaySpecifyingType));
            Assert.That(source.Recurring.DurationUnit, Is.EqualTo(dest.Recurring.DurationUnit));
            Assert.That(source.Recurring.DurationValue, Is.EqualTo(dest.Recurring.DurationValue));
            Assert.That(source.Recurring.Month, Is.EqualTo(dest.Recurring.Month));
            Assert.That(source.Recurring.OnFriday, Is.EqualTo(dest.Recurring.OnFriday));
            Assert.That(source.Recurring.OnMonday, Is.EqualTo(dest.Recurring.OnMonday));
            Assert.That(source.Recurring.OnSaturday, Is.EqualTo(dest.Recurring.OnSaturday));
            Assert.That(source.Recurring.OnSunday, Is.EqualTo(dest.Recurring.OnSunday));
            Assert.That(source.Recurring.OnThursday, Is.EqualTo(dest.Recurring.OnThursday));
            Assert.That(source.Recurring.OnTuesday, Is.EqualTo(dest.Recurring.OnTuesday));
            Assert.That(source.Recurring.OnWednesday, Is.EqualTo(dest.Recurring.OnWednesday));
            Assert.That(source.Recurring.RepetitionEndDate, Is.EqualTo(dest.Recurring.RepetitionEndDate));
            Assert.That(source.Recurring.RepetitionNoEnd, Is.EqualTo(dest.Recurring.RepetitionNoEnd));
            Assert.That(source.Recurring.RepetitionStartDate, Is.EqualTo(dest.Recurring.RepetitionStartDate));
            Assert.That(source.Recurring.ScheduleRecurringType, Is.EqualTo(dest.Recurring.ScheduleRecurringType));
            Assert.That(source.Recurring.ScheduleRecurringValue, Is.EqualTo(dest.Recurring.ScheduleRecurringValue));
            Assert.That(source.Recurring.ShowEndTime, Is.EqualTo(dest.Recurring.ShowEndTime));
            Assert.That(source.Recurring.ShowLimitationType, Is.EqualTo(dest.Recurring.ShowLimitationType));
            Assert.That(source.Recurring.ShowStartTime, Is.EqualTo(dest.Recurring.ShowStartTime));
            Assert.That(source.Recurring.WeekOfMonth, Is.EqualTo(dest.Recurring.WeekOfMonth));
        }
    }
}
