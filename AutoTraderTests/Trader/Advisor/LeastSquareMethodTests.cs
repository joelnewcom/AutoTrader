using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AutoTrader.Advisor;
using System;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class LeastSquareMethodTests
    {

        [TestMethod()]
        public void SimpleLinearTest()
        {
            List<Decimal> y = new List<Decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Decimal slope = LeastSquareMethod.Slope(y);
            Assert.AreEqual(1, slope);
        }

        [TestMethod()]
        public void SimpleNegativeLinearTest()
        {
            List<Decimal> y = new List<Decimal> { 5, 4, 3, 2, 1 };
            Decimal slope = LeastSquareMethod.Slope(y);
            Assert.AreEqual(-1, slope);
        }
    }
}