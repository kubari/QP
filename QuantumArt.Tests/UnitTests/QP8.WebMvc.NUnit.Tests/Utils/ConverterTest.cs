using System;
using System.Globalization;
using NUnit.Framework;
using Quantumart.QP8.Utils;

namespace QP8.WebMvc.NUnit.Tests.Utils
{
    [TestFixture]
    public class ConverterTest
    {
        [Test]
        public void TryConvertToSqlDateString_NotNullTimeAndCorrectFormat_CorrectResult()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var result = Converter.TryConvertToSqlDateString("5/17/2011", new TimeSpan(23, 59, 59), out var sqlDateString, out var dateTime);

            Assert.That(result, Is.True);
            Assert.That(dateTime, Is.Not.Null);
            Assert.That(new DateTime(2011, 5, 17, 23, 59, 59), Is.EqualTo(dateTime));
            Assert.That("20110517 23:59:59", Is.EqualTo(sqlDateString));
        }

        [Test]
        public void TryConvertToSqlDateString_NullTimeAndCorrectFormat_CorrectResult()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            var result = Converter.TryConvertToSqlDateString("5/17/2011", null, out var sqlDateString, out var dateTime);

            Assert.That(result, Is.True);
            Assert.That(dateTime, Is.Not.Null);
            Assert.That(new DateTime(2011, 5, 17, 0, 0, 0), Is.EqualTo(dateTime));
            Assert.That("20110517 00:00:00", Is.EqualTo(sqlDateString));
        }

        [Test]
        public void TryConvertToSqlDateString_IncorrectFormat_EmptyResult()
        {
            var result = Converter.TryConvertToSqlDateString("2011/17/5", null, out var sqlDateString, out var dateTime);

            Assert.That(result, Is.False);
            Assert.That(dateTime, Is.Null);
            Assert.That(string.Empty, Is.EqualTo(sqlDateString));
        }

        [Test]
        public void TryConvertToSqlTimeString_CorrectFormat_CorrectResult()
        {
            var result = Converter.TryConvertToSqlTimeString("10:58:41 PM", out var sqlTimeString, out var timeSpan);

            Assert.That(result, Is.True);
            Assert.That(timeSpan, Is.Not.Null);
            Assert.That(new TimeSpan(22, 58, 41), Is.EqualTo(timeSpan));
            Assert.That("22:58:41", Is.EqualTo(sqlTimeString));
        }

        [Test]
        public void TryConvertToSqlTimeString_IncorrectFormat_EmptyResult()
        {
            var result = Converter.TryConvertToSqlTimeString("22:58 PM", out var sqlTimeString, out var timeSpan, "h:mm:ss tt");

            Assert.That(result, Is.False);
            Assert.That(timeSpan, Is.Null);
            Assert.That(string.Empty, Is.EqualTo(sqlTimeString));
        }
    }
}
