using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Simplic.CoreLib.PreciseDecimal
{
    [TestClass]
    public class CompareToTest
    {
        [TestMethod]
        public void CompareToPreciseDecimalLesser()
        {
            Data.PreciseDecimal val1 = 20;
            Data.PreciseDecimal val2 = 25;

            Assert.AreEqual(-1, val1.CompareTo(val2));
        }

        [TestMethod]
        public void CompareToPreciseDecimalEqual()
        {
            Data.PreciseDecimal val1 = 100;
            Data.PreciseDecimal val2 = 100;

            Assert.AreEqual(0, val1.CompareTo(val2));
        }

        [TestMethod]
        public void CompareToPreciseDecimalGreater()
        {
            Data.PreciseDecimal val1 = 19;
            Data.PreciseDecimal val2 = 5;

            Assert.AreEqual(1, val1.CompareTo(val2));
        }
    }
}
