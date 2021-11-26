using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using AutoTrader.Advisor;
using Moq;
using AutoTrader.Repository;
using AutoTrader.Data;
using System.Threading.Tasks;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class AlwaysWinSellerTests
    {

        readonly Mock<IRepository> repoMock = new Mock<IRepository>();

        [TestMethod()]
        public async Task LatestBuyTradeWithHigherPriceLeadsToHoldOn()
        {
            repoMock.Setup(repo => repo.GetPrice("ETHCHF")).ReturnsAsync(new Price(System.DateTime.Now, 10, 10));

            repoMock.Setup(repo => repo.GetTrades()).ReturnsAsync(new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now, "ETHCHF", 11, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 9, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF"))
                }
            );

            AlwaysWinSeller alwaysWinSeller = new AlwaysWinSeller(repoMock.Object);
            Advice advice = await alwaysWinSeller.advice("ETHCHF");

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [TestMethod()]
        public async Task LatestBuyTradeWithLowerPriceLeadsToHoldOn()
        {
            repoMock.Setup(repo => repo.GetPrice("ETHCHF")).ReturnsAsync(new Price(System.DateTime.Now, 10, 10));

            repoMock.Setup(repo => repo.GetTrades()).ReturnsAsync(new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now, "ETHCHF", 9, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 11, "CHF", "ETH", "Taker", "buy", 100, 900, new TradeFee(0, "CHF"))
                }
            );

            AlwaysWinSeller alwaysWinSeller = new AlwaysWinSeller(repoMock.Object);
            Advice advice = await alwaysWinSeller.advice("ETHCHF");

            Assert.AreEqual(Advice.Sell, advice);
        }

    }
}