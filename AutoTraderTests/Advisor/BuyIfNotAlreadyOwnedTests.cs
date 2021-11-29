using System.Collections.Generic;
using AutoTrader.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTrader.Advisor
{
    [TestClass()]
    public class BuyIfNotAlreadyOwnedTests
    {

        [TestMethod()]
        public void DontBuyIfAlreadyOwned()
        {
            BuyIfNotAlreadyOwned buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned();
            Advice advice = buyIfNotAlreadyOwned.advice("LKK", new List<IBalance>()
            {
                new Balance("LKK", 100, 100)
            });

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [TestMethod()]
        public void BuyIfAlreadyOwned()
        {
            BuyIfNotAlreadyOwned buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned();
            Advice advice = buyIfNotAlreadyOwned.advice("Eur", new List<IBalance>()
            {
                new Balance("LKK", 100, 100),
                new Balance("CHF", 100, 100),
                new Balance("BTC", 100, 100),
            });

            Assert.AreEqual(Advice.Buy, advice);
        }
    }
}