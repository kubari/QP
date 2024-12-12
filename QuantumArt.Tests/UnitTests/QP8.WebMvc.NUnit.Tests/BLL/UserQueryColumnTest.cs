using System.Linq;
using NUnit.Framework;
using Quantumart.QP8.BLL;

namespace QP8.WebMvc.NUnit.Tests.BLL
{
    [TestFixture]
    public class UserQueryColumnTest
    {
        [Test]
        public void IgnoreTableNameComparationTest()
        {
            var collection1 = new[]
            {
                new UserQueryColumn { ColumnName = "C1", DbType = "Datetime", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C2", DbType = "datetime", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C3", DbType = "nvarchar", TableName = "T1" },
                new UserQueryColumn { ColumnName = null, DbType = "numeric", TableName = null }
            };

            var collection2 = new[]
            {
                new UserQueryColumn { ColumnName = "c1", DbType = "datetime", TableName = "T2" },
                new UserQueryColumn { ColumnName = "C2", DbType = "numeric", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C4", DbType = "numeric", TableName = "T1" },
                new UserQueryColumn { ColumnName = "c3", DbType = null, TableName = "T1" }
            };

            var intersect = collection1.Intersect(collection2, UserQueryColumn.TableNameIgnoreEqualityComparer).ToArray();

            Assert.That(1, Is.EqualTo(intersect.Length));
            Assert.That(intersect[0].ColumnName.ToLowerInvariant(), Is.EqualTo("c1"));
            Assert.That(intersect[0].DbType.ToLowerInvariant(), Is.EqualTo("datetime"));
        }

        [Test]
        public void ComparationTest()
        {
            var collection1 = new[]
            {
                new UserQueryColumn { ColumnName = "C1", DbType = "Datetime", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C2", DbType = "datetime", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C3", DbType = "nvarchar", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C4", DbType = "numeric", TableName = "T1" },
                new UserQueryColumn { ColumnName = null, DbType = null, TableName = "T1" }
            };

            var collection2 = new[]
            {
                new UserQueryColumn { ColumnName = "C1", DbType = "datetime", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C2", DbType = "numeric", TableName = "T1" },
                new UserQueryColumn { ColumnName = "C3", DbType = "nvarchar", TableName = "T2" },
                new UserQueryColumn { ColumnName = "C5", DbType = "numeric", TableName = "T1" },
                new UserQueryColumn { ColumnName = null, DbType = null, TableName = "T2" }
            };

            var intersect = collection1.Intersect(collection2).ToArray();

            Assert.That(1, Is.EqualTo(intersect.Length));
            Assert.That(intersect[0].ColumnName.ToLowerInvariant(), Is.EqualTo("c1"));
            Assert.That(intersect[0].DbType.ToLowerInvariant(), Is.EqualTo("datetime"));
        }

        [Test]
        public void SelectColumnCompareToTest()
        {
            var source = new[]
            {
                new UserQueryColumn { TableName = "content_10" },
                new UserQueryColumn { TableName = "t1" },
                new UserQueryColumn { TableName = null },
                new UserQueryColumn { TableName = "t0" },
                new UserQueryColumn { TableName = "content_0" }
            };

            var result = source.OrderByDescending(c => c, UserQueryColumn.SelectBaseColumnComparer).ToArray();

            Assert.That(source.Length, Is.EqualTo(result.Length));
            Assert.That("content_0", Is.EqualTo(result[0].TableName));
            Assert.That("content_10", Is.EqualTo(result[1].TableName));
            Assert.That("t1", Is.EqualTo(result[2].TableName));
            Assert.That("t0", Is.EqualTo(result[3].TableName));
            Assert.That(result[4].TableName, Is.Null);
        }
    }
}
