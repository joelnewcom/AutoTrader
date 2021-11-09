using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Library;
using System;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class LeastSquareMethodTests
    {

        [TestMethod()]
        public void SimpleLinearTest()
        {
            // given
            List<float> y = new List<float> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            // when
            float slope = new LeastSquareMethod().Slope(y);

            // then
            Assert.AreEqual(1, slope);
        }

        [TestMethod()]
        public void SimpleNegativeLinearTest()
        {
            // given
            List<float> y = new List<float> { 5,4,3,2,1 };

            // when
            float slope = new LeastSquareMethod().Slope(y);

            // then
            Assert.AreEqual(-1, slope);
        }
    }
}