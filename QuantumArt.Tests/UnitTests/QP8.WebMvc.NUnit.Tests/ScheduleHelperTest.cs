using System;
using NUnit.Framework;
using Quantumart.QP8.BLL.Helpers;

namespace QP8.WebMvc.NUnit.Tests
{
    [TestFixture]
    public class ScheduleHelperTest
    {
        [Test]
        public void GetSqlValuesFromScheduleDateTest()
        {
            var dt = new DateTime(2010, 3, 9, 5, 6, 7);
            var values = ScheduleHelper.GetSqlValuesFromScheduleDateTime(dt);
            Assert.That(20100309, Is.EqualTo(values.Item1));
            Assert.That(50607, Is.EqualTo(values.Item1));
        }

        [Test]
        public void GetSqlValuesFromScheduleDateTest2()
        {
            var dt = new DateTime(2010, 3, 9, 15, 6, 7);
            var values = ScheduleHelper.GetSqlValuesFromScheduleDateTime(dt);
            Assert.That(20100309, Is.EqualTo(values.Item1));
            Assert.That(150607, Is.EqualTo(values.Item2));
        }

        [Test]
        public void GetScheduleDateFromSqlValuesTest()
        {
            const int sqlDate = 20100309;
            var dt = ScheduleHelper.GetScheduleDateFromSqlValues(sqlDate);
            Assert.That(new DateTime(2010, 3, 9), Is.EqualTo(dt));
        }

        [Test]
        public void GetScheduleTimeFromSqlValuesTest()
        {
            const int sqlTime = 50607;
            var t = ScheduleHelper.GetScheduleTimeFromSqlValues(sqlTime);

            Assert.That(new TimeSpan(5, 6, 7), Is.EqualTo(t));
        }

        [Test]
        public void GetScheduleDateTimeFromSqlValuesTest()
        {
            const int sqlDate = 20100309;
            const int sqlTime = 50607;
            var dt = ScheduleHelper.GetScheduleDateTimeFromSqlValues(sqlDate, sqlTime);
            Assert.That(2010, Is.EqualTo(dt.Year));
            Assert.That(3, Is.EqualTo(dt.Month));
            Assert.That(9, Is.EqualTo(dt.Day));
            Assert.That(5, Is.EqualTo(dt.Hour));
            Assert.That(6, Is.EqualTo(dt.Minute));
            Assert.That(7, Is.EqualTo(dt.Second));
        }

        [Test]
        public void GetScheduleDateTimeFromSqlValuesTest2()
        {
            const int sqlDate = 20100309;
            const int sqlTime = 150607;
            var dt = ScheduleHelper.GetScheduleDateTimeFromSqlValues(sqlDate, sqlTime);
            Assert.That(15, Is.EqualTo(dt.Hour));
        }

        [Test]
        public void GetScheduleDateTimeFromSqlValuesTest3()
        {
            const int sqlDate = 20100309;
            const int sqlTime = 0;
            var dt = ScheduleHelper.GetScheduleDateTimeFromSqlValues(sqlDate, sqlTime);
            Assert.That(0, Is.EqualTo(dt.Hour));
            Assert.That(0, Is.EqualTo(dt.Minute));
            Assert.That(0, Is.EqualTo(dt.Second));
        }

        [Test]
        public void GetDurationTest()
        {
            var duration = ScheduleHelper.GetDuration("mi", 1, DateTime.Now);
            Assert.That(TimeSpan.FromMinutes(1), Is.EqualTo(duration));

            duration = ScheduleHelper.GetDuration("hh", 1, DateTime.Now);
            Assert.That(TimeSpan.FromHours(1), Is.EqualTo(duration));

            duration = ScheduleHelper.GetDuration("dd", 1, DateTime.Now);
            Assert.That(TimeSpan.FromDays(1), Is.EqualTo(duration));

            duration = ScheduleHelper.GetDuration("wk", 2, DateTime.Now);
            Assert.That(TimeSpan.FromDays(14), Is.EqualTo(duration));

            var startDate = new DateTime(2011, 1, 1, 12, 15, 17);

            duration = ScheduleHelper.GetDuration("mm", 2, startDate);
            Assert.That(new DateTime(2011, 3, 1), Is.EqualTo((startDate + duration).Date));

            duration = ScheduleHelper.GetDuration("yy", 2, startDate);
            Assert.That(new DateTime(2013, 1, 1), Is.EqualTo((startDate + duration).Date));
        }
    }
}
