using NUnit.Framework;
using Quantumart.QP8.BLL.Helpers;

namespace QP8.WebMvc.NUnit.Tests.BLL.Helpers
{
    [TestFixture]
    public class MultistepActionHelperTest
    {
        [Test]
        public void GetStepCountTest()
        {
            Assert.That(0, Is.EqualTo(MultistepActionHelper.GetStepCount(0, 20)));
            Assert.That(1, Is.EqualTo(MultistepActionHelper.GetStepCount(1, 20)));
            Assert.That(1, Is.EqualTo(MultistepActionHelper.GetStepCount(19, 20)));
            Assert.That(1, Is.EqualTo(MultistepActionHelper.GetStepCount(20, 20)));
            Assert.That(2, Is.EqualTo(MultistepActionHelper.GetStepCount(21, 20)));
            Assert.That(5, Is.EqualTo(MultistepActionHelper.GetStepCount(100, 20)));
            Assert.That(6, Is.EqualTo(MultistepActionHelper.GetStepCount(101, 20)));
        }
    }
}
