using System;
using NUnit.Framework;
using Quantumart.QP8.Utils;

namespace QP8.WebMvc.NUnit.Tests.Utils
{
    [TestFixture]
    public class InitPropertyValueTest
    {
        private readonly Func<int> _initializer;
        private readonly Func<int, int> _customSetter;

        private const int InitializerResult = 100;
        private const int CustomSetterResult = 200;

        public InitPropertyValueTest()
        {
            _initializer = () => InitializerResult;
            _customSetter = v => CustomSetterResult;
        }

        public TestContext TestContext { get; set; }

        [Test]
        public void GetterSetterCheck_InitializerIsDefinedAndCustomSetterIsNull_CorrectResult()
        {
            var value = new InitPropertyValue<int>(_initializer);
            Assert.That(InitializerResult, Is.EqualTo(value.Value));

            const int newValue = 3 * InitializerResult;
            value.Value = newValue;
            Assert.That(newValue, Is.EqualTo(value.Value));
        }

        [Test]
        public void GetterSetterCheck_InitializerAndCustomSetterIsDefined_CorrectResult()
        {
            var value = new InitPropertyValue<int>(_initializer, _customSetter);
            Assert.That(InitializerResult, Is.EqualTo(value.Value));

            const int newValue = 3 * InitializerResult;
            value.Value = newValue;
            Assert.That(CustomSetterResult, Is.EqualTo(value.Value));
        }

        [Test]
        public void GetterSetterCheck_InitializerAndCustomSetterIsUndefined_CorrectResult()
        {
            var value = new InitPropertyValue<int>();

            // нет инициализации - значение по умолчанию
            Assert.That(default(int), Is.EqualTo(value.Value));

            // Устанавливаем initializer
            value.Initializer = _initializer;

            Assert.That(InitializerResult, Is.EqualTo(value.Value));

            const int newValue = 3 * InitializerResult;
            value.Value = newValue;
            Assert.That(newValue, Is.EqualTo(value.Value));
        }

        [Test]
        public void GetterSetterCheck_InitializerAndCustomSetterDefineLater_CorrectResult()
        {
            var value = new InitPropertyValue<int>();

            // нет инициализации - значение по умолчанию
            Assert.That(default(int), Is.EqualTo(value.Value));

            // Проверка get/set
            const int newValue = 3 * InitializerResult;
            value.Value = newValue;
            Assert.That(newValue, Is.EqualTo(value.Value));

            // Устанавливаем initializer
            value.Initializer = _initializer;

            // Так как уже был set, то initializer не применяется
            Assert.That(newValue, Is.EqualTo(value.Value));
        }
    }
}
