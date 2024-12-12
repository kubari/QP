using NUnit.Framework;
using Quantumart.QP8.Constants;
using Quantumart.QP8.Utils;

namespace QP8.WebMvc.NUnit.Tests.Utils
{
    [TestFixture]
    public class CleanerTest
    {
        [Test]
        public void ToSafeSqlLikeCondition_NullInput_NullResult()
        {
            Assert.That(Cleaner.ToSafeSqlLikeCondition(DatabaseType.SqlServer, null), Is.Null);
        }

        [Test]
        public void ToSafeSqlLikeCondition_SafeInput_SafeResult()
        {
            Assert.That("test", Is.EqualTo(Cleaner.ToSafeSqlLikeCondition(DatabaseType.SqlServer,"test")));
        }

        [Test]
        public void ToSafeSqlLikeCondition_UnsafeInput_SafeResult()
        {
            Assert.That("'' [[] [%] [_]", Is.EqualTo(Cleaner.ToSafeSqlLikeCondition(DatabaseType.SqlServer,"' [ % _")));
        }

        [Test]
        public void ToSafeSqlLikeConditionPg_UnsafeInput_SafeResult()
        {
            Assert.That(@"'' [ \% \_", Is.EqualTo(Cleaner.ToSafeSqlLikeCondition(DatabaseType.Postgres,"' [ % _")));
        }

        [Test]
        public void ToSafeSqlString_NullInput_NullResult()
        {
            Assert.That(Cleaner.ToSafeSqlString(null), Is.Null);
        }

        [Test]
        public void ToSafeSqlString_SafeInput_SafeResult()
        {
            Assert.That("test", Is.EqualTo(Cleaner.ToSafeSqlString("test")));
        }

        [Test]
        public void ToSafeSqlString_UnsafeInput_SafeResult()
        {
            Assert.That("''test''", Is.EqualTo(Cleaner.ToSafeSqlString("'test'")));
        }
    }
}
