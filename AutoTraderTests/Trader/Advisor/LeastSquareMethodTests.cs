using System;
using System.Collections.Generic;
using AutoTrader.Advisor;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoTraderTests.Trader.Advisor
{
    public class LeastSquareMethodTests
    {

        [Fact]
        public void SimpleLinearTest()
        {
            List<Decimal> y = new List<Decimal> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Decimal slope = LeastSquareMethod.Slope(y);
            Assert.AreEqual(1, slope);
        }

        [Fact]
        public void SimpleNegativeLinearTest()
        {
            List<Decimal> y = new List<Decimal> { 5, 4, 3, 2, 1 };
            Decimal slope = LeastSquareMethod.Slope(y);
            Assert.AreEqual(-1, slope);
        }
    }
}