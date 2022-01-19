using System.Collections.Generic;
using AutoTrader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrader.Advisor
{
    [TestClass()]
    public class SellIfAlreadyOwnedTests
    {

        [TestMethod()]
        public void SellIfAlreadyOwned()
        {
            SellIfAlreadyOwned sellIfAlreadyOwned = new SellIfAlreadyOwned();
            Advice advice = sellIfAlreadyOwned.advice("LKK", new List<IBalance>()
            {
                new Balance("LKK", 100, 99)
            });

            Assert.AreEqual(Advice.Sell, advice);
        }

        [TestMethod()]
        public void HoldOnIfAlreadyOwnedButReserved()
        {
            SellIfAlreadyOwned sellIfAlreadyOwned = new SellIfAlreadyOwned();
            Advice advice = sellIfAlreadyOwned.advice("LKK", new List<IBalance>()
            {
                new Balance("LKK", 50, 50)
            });

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [TestMethod()]
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