using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Text;
using System.Globalization;

namespace Simplic.CoreLib.StringExtraction
{
    [TestClass]
    public class FindInLineMatchTest
    {
        [TestMethod]
        public void FindInLineNumberTest()
        {
            string demo = @"
                DEMO STRING
                Endsumme   _   EUR         123,28
                DEMO STRING
            ";

            var result = Text.StringExtraction.FindInLine(demo, new[] { new ExtractionKey { Key = "Endsumme" } }, "", (value) =>
            {
                try
                {
                    Text.StringExtraction.CastAsNumber(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            Assert.AreEqual("123,28", result.OriginalValue);
            Assert.AreEqual("123,28", result.Value.Value);
        }

        [TestMethod]
        public void FindInLineDateTest()
        {
            string demo = @"
                DEMO STRING
                Datum   Test/Datum2 07.09.2019 T
                DEMO STRING
            ";

            var result = Text.StringExtraction.FindInLine(demo, new[] { new ExtractionKey { Key = "Datum" } }, "", (value) =>
            {
                try
                {
                    DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            Assert.AreEqual("07.09.2019", result.OriginalValue);
            Assert.AreEqual("07.09.2019", result.Value.Value);
        }

        [TestMethod]
        public void FindInLineDateWithSpaceTest()
        {
            string demo = @"
                DEMO STRING
                Rechnungs Datum   Test/Datum2 07.09.2019 T
                DEMO STRING
            ";

            var result = Text.StringExtraction.FindInLine(demo, new[] { new ExtractionKey { Key = "Rechnungs Datum" } }, "", (value) =>
            {
                try
                {
                    DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            Assert.AreEqual("07.09.2019", result.OriginalValue);
            Assert.AreEqual("07.09.2019", result.Value.Value);
        }

        [TestMethod]
        public void FindInNextLine()
        {
            string demo = @"
                Auftragsnr.            USt-ldNr.               Ihre Kundennr.       Ihre Referenz                                            Datum
                VA0068171                       DE278590960                 33548                                  Lager                                                                                          02.08.2018

            ";

            var result = Text.StringExtraction.FindInNextLine(demo, new[] { new ExtractionKey { Key = "Datum" } }, "", (value) =>
            {
                try
                {
                    DateTime.ParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    return true;
                }
                catch
                {
                    return false;
                }
            });

            Assert.AreEqual("02.08.2018", result.OriginalValue);
            Assert.AreEqual("02.08.2018", result.Value.Value);
        }

        [TestMethod]
        public void FindInLineCustomerNr()
        {
            string demo = @"
                DEMO STRING
                Tief- & Straßenbau GmbH              Kunden-Nr. :      110030
                DEMO STRING
            ";

            var id = Guid.NewGuid();
            var result = Text.StringExtraction.FindInLine(demo, new[] { new ExtractionKey { Key = "Kunden-Nr." }, new ExtractionKey { Key = "Kunden-Nr" } }, new[] { new ExtractionValue { Id = id, Value = "110030" } }, forceWhiteList: true);

            Assert.AreEqual("110030", result.OriginalValue);
            Assert.AreEqual(id, result.Value.Id);
        }

        [TestMethod]
        public void FindInLineMinLength()
        {
            string demo = @"
                DEMO STRING
                Beleg-Nr. : 7125567
                DEMO STRING
            ";

            var id = Guid.NewGuid();
            var result = Text.StringExtraction.FindInLine(demo, new[] { new ExtractionKey { Key = "Beleg-Nr." } }, "", minResultLenght: 3);

            Assert.AreEqual("7125567", result.OriginalValue);
        }

        [TestMethod]
        public void SplitLineTest()
        {
            string demo = "1    8242            Diverse  Deponiegebühren                                   2 , 38       to           3 , 00     / E          7 , 14       1";

            var splittedLine = Text.StringExtraction.SplitLine(demo, ' ', 3, 0);

            Assert.AreEqual(splittedLine.Count, 9);
        }
    }
}