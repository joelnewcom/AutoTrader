using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AutoTrader.Advisor;
using AutoTrader.Data;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class AlwaysWinSellerTests
    {

        [TestMethod()]
        public void LatestBuyTradeWithHigherPriceLeadsToHoldOn()
        {
            AlwaysWinSeller alwaysWinSeller = new AlwaysWinSeller();
            Advice advice = alwaysWinSeller.advice("ETHCHF", new Price(System.DateTime.Now, 10, 10, "ETHCHF"), new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now, "ETHCHF", 11, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 9, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF"))
                });

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [TestMethod()]
        public void LatestBuyTradeWithLowerPriceLeadsToHoldOn()
        {
            AlwaysWinSeller alwaysWinSeller = new AlwaysWinSeller();
            Advice advice = alwaysWinSeller.advice("ETHCHF", new Price(System.DateTime.Now, 10, 10, "ETHCHF"), new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now, "ETHCHF", 9, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 11, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF"))
                });

            Assert.AreEqual(Advice.Sell, advice);
        }

    }
}