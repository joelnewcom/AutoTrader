using System.Collections.Generic;
using AutoTrader.Advisor;
using AutoTrader.Models;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoTraderTests.Trader.Advisor
{
    public class SellIfAlreadyOwnedTests
    {

        [Fact]
        public void SellIfAlreadyOwned()
        {
            SellIfAlreadyOwned sellIfAlreadyOwned = new SellIfAlreadyOwned();
            Advice advice = sellIfAlreadyOwned.advice("LKK", new List<IBalance>()
            {
                new Balance("LKK", 100, 99)
            });

            Assert.AreEqual(Advice.Sell, advice);
        }

        [Fact]
        public void HoldOnIfAlreadyOwnedButReserved()
        {
            SellIfAlreadyOwned sellIfAlreadyOwned = new SellIfAlreadyOwned();
            Advice advice = sellIfAlreadyOwned.advice("LKK", new List<IBalance>()
            {
                new Balance("LKK", 50, 50)
            });

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [Fact]
        public void SellIfNotAlreadyOwned()
        {
            SellIfAlreadyOwned sellIfAlreadyOwned = new SellIfAlreadyOwned();
            Advice advice = sellIfAlreadyOwned.advice("Eur", new List<IBalance>()
            {
                new Balance("LKK", 100, 100),
                new Balance("CHF", 100, 100),
                new Balance("BTC", 100, 100),
            });

            Assert.AreEqual(Advice.HoldOn, advice);
        }
    }
}