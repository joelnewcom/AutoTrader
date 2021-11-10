using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AutoTrader.Advisor;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class LinearSlopeAdvisorTests
    {
        IAdvisor advisor = new LinearSlopeAdvisor();

        [TestMethod()]
        public void ShouldBuy_GoingDownOverallButUpAtTail()
        {
            Assert.AreEqual(Advice.Buy, advisor.advice(new List<float> { 7, 6, 5, 4, 3, 5, 6 }));
        }

        [TestMethod()]
        public void HoldOn_GoingDownOverallButStableAtTail()
        {
            Assert.AreEqual(Advice.HoldOn, advisor.advice(new List<float> { 7, 6, 5, 4, 3, 5, 5 }));
        }

        [TestMethod()]
        public void ShouldSell_GoingUpOverallButDownAtTail()
        {
            Assert.AreEqual(Advice.Sell, advisor.advice(new List<float> { 1, 2, 3, 4, 5, 3, 2 }));
        }

        [TestMethod()]
        public void HoldOn_GoingUpOverallButStableTail()
        {
            Assert.AreEqual(Advice.HoldOn, advisor.advice(new List<float> { 1, 2, 3, 4, 5, 3, 3 }));
        }
    }
}