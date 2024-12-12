using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Quantumart.QP8.Utils.FullTextSearch;

namespace QP8.WebMvc.NUnit.Tests.Utils.FullTextSearch
{
    [TestFixture]
    public class FoundTextMarkerTest
    {
        [Test]
        public void FindWordFormPositionDictionaryTest()
        {
            IEnumerable<string> forms = new[]
            {
                "тарифы",
                "тарифу",
                "тарифом",
                "тарифов",
                "тарифе",
                "тарифах",
                "тарифами",
                "тарифам",
                "тарифа",
                "тариф",
                "интернету",
                "интернетом",
                "интернете",
                "интернета",
                "интернет"
            };

            var res = FoundTextMarker.GetRelevantMarkedText("ЭТОТ ТАРИФ НА ИНТЕРНЕТ ОЧЕНЬ ВЫГОДЕН", forms, 10, "<b>", "</b>");
            Assert.That("ЭТОТ <b>ТАРИФ</b> НА <b>ИНТЕРНЕТ</b>", Is.EqualTo(res));

            res = FoundTextMarker.GetRelevantMarkedText("Есть много новых тарифов которые стоят не дорого.", forms, 10, "<b>", "</b>");
            Assert.That("много новых <b>тарифов</b> которые стоят", Is.EqualTo(res));

            res = FoundTextMarker.GetRelevantMarkedText("Сейчас очень быстрый доступ в ИНТЕРНЕТ.", forms, 10, "<b>", "</b>");
            Assert.That("быстрый доступ в <b>ИНТЕРНЕТ</b>.", Is.EqualTo(res));

            res = FoundTextMarker.GetRelevantMarkedText("Вообще левая строка", forms, 10, "<b>", "</b>");
            Assert.That("Вообще левая", Is.EqualTo(res));

            res = FoundTextMarker.GetRelevantMarkedText("", forms, 10, "<b>", "</b>");
            Assert.That(string.Empty, Is.EqualTo(res));

            res = FoundTextMarker.GetRelevantMarkedText(null, forms, 10, "<b>", "</b>");
            Assert.That(res, Is.Null);

            res = FoundTextMarker.GetRelevantMarkedText("этот тариф на интернет очень выгоден", Enumerable.Empty<string>(), 10, "<b>", "</b>");
            Assert.That("этот тариф", Is.EqualTo(res));
        }
    }
}
