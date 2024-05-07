using System;
using System.Collections.Generic;
using AutoTrader.Advisor;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoTraderTests.Trader.Advisor
{
    public class LinearSlopeAdvisorTests
    {
        IAdvisor<List<Decimal>> advisor = new LinearSlopeAdvisor();

        [Fact]
        public void ShouldBuy_GoingDownOverallButUpAtTail()
        {
            Assert.AreEqual(Advice.Buy, advisor.advice(new List<Decimal> { 7, 6, 5, 4, 3, 5, 6 }));
        }

        [Fact]
        public void HoldOn_GoingDownOverallButStableAtTail()
        {
            Assert.AreEqual(Advice.HoldOn, advisor.advice(new List<Decimal> { 7, 6, 5, 4, 3, 5, 5 }));
        }

        [Fact]
        public void ShouldSell_GoingUpOverallButDownAtTail()
        {
            Assert.AreEqual(Advice.Sell, advisor.advice(new List<Decimal> { 1, 2, 3, 4, 5, 3, 2 }));
        }

        [Fact]
        public void HoldOn_GoingUpOverallButStableTail()
        {
            Assert.AreEqual(Advice.HoldOn, advisor.advice(new List<Decimal> { 1, 2, 3, 4, 5, 3, 3 }));
        }

        //0.00899, 0.0085, 0.00767, 0.0085, 0.00833, 0.00767, 0.00766

        
        [Fact]
        public void HoldOn_GoingDownOverallButStableTail()
        {
            Assert.AreEqual(Advice.HoldOn, advisor.advice(new List<Decimal> {0.00899m, 0.0085m, 0.00767m, 0.0085m, 0.00833m, 0.00767m, 0.00766m }));
        }
    }
}