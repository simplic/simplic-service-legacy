using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simplic.Text;
using System.Globalization;
using System.Text;

namespace Simplic.CoreLib.StringExtraction
{
    [TestClass]
    public class CastAsNumberTest
    {
        [TestMethod]
        public void TestInvalidCast()
        {
            try
            {
                Text.StringExtraction.CastAsNumber("BLZ 123.929.2929,123");
                Assert.Fail();
            }
            catch (InvalidCastException)
            {
                
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestSpaceInNumber()
        {

            var asci = Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(((char)(8218)).ToString()))[0];


            var val = Text.StringExtraction.CastAsNumber("71 ‚80");
            Assert.AreEqual(val, 71.80);
        }
    }
}