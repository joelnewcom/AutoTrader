using System.Collections.Generic;
using AutoTrader.Config;
using AutoTrader.Models;
using AutoTrader.Trader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoTraderTests.Trader
{
    [TestClass()]
    public class TraderServiceTests
    {
        readonly Mock<IServiceScopeFactory> dependencyInjectionScope = new Mock<IServiceScopeFactory>();
        readonly Mock<TraderConfig> config = new Mock<TraderConfig>();
        readonly Mock<ILogger<TraderService>> logger = new Mock<ILogger<TraderService>>();

        TraderService traderService;
        public TraderServiceTests()
        {
            config.Setup(p => p.safetyCatch).Returns(new SafetyCatch() { buySafetyCatch = true, sellSafetyCatch = true });
            traderService = new TraderService(logger.Object, config.Object, dependencyInjectionScope.Object);
        }

        [TestMethod()]
        public void OnlyBuysInPast()
        {
            List<TradeEntry> trades = new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-2), "ETHCHF", 10, "ETH", "CHF", "Taker", "buy", 11, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 15, "ETH", "CHF", "Taker", "buy", 12, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-2), "BTCCHF", 10, "BTC", "CHF", "Taker", "buy", 13, 0, new TradeFee(0, "CHF"))
                };
            List<IBalance> balances = new List<IBalance>() { new Balance("ETH", 25, 0) };
            AssetPair assetPair = new AssetPair("ETHCHF", "testAssetPair", 1000, "ETH", "CHF", 0, 0, 0);
            Price price = new Price(System.DateTime.Now, 12, 12, "ETHCHF");

            Assert.AreEqual(11, traderService.getMaxVolumeToSellAlwaysWin(trades, balances, assetPair, price));
        }

        [TestMethod()]
        public void SoldAlreadyInThePast()
        {
            List<TradeEntry> trades = new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-4), "ETHCHF", 12, "ETH", "CHF", "Taker", "buy", 10, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-2), "ETHCHF", 2, "ETH", "CHF", "Taker", "buy", 5, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-1), "ETHCHF", 5, "ETH", "CHF", "Taker", "sell", 5, 0, new TradeFee(0, "CHF"))
                };
            List<IBalance> balances = new List<IBalance>() { new Balance("ETH", 10, 0) };
            AssetPair assetPair = new AssetPair("ETHCHF", "testAssetPair", 1000, "ETH", "CHF", 0, 0, 0);
            Price price = new Price(System.DateTime.Now, 11, 11, "ETHCHF");

            Assert.AreEqual(0, traderService.getMaxVolumeToSellAlwaysWin(trades, balances, assetPair, price));
        }

        [TestMethod()]
        public void SoldAlreadyMutipleTimesInThePast()
        {
            List<TradeEntry> trades = new List<TradeEntry>(){
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-7), "ETHCHF", 8, "ETH", "CHF", "Taker", "buy", 3, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-6), "ETHCHF", 10, "ETH", "CHF", "Taker", "buy", 5, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-5), "ETHCHF", 20, "ETH", "CHF", "Taker", "buy", 8, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-4), "ETHCHF", 9, "ETH", "CHF", "Taker", "sell", 3, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-3), "ETHCHF", 11, "ETH", "CHF", "Taker", "sell", 5, 0, new TradeFee(0, "CHF")),
                    new TradeEntry("uuid", System.DateTime.Now.AddDays(-2), "ETHCHF", 20, "ETH", "CHF", "Taker", "buy", 5, 0, new TradeFee(0, "CHF")),
                };
            List<IBalance> balances = new List<IBalance>() { new Balance("ETH", 20, 0) };
            AssetPair assetPair = new AssetPair("ETHCHF", "testAssetPair", 1000, "ETH", "CHF", 0, 0, 0);
            Price price = new Price(System.DateTime.Now, 21, 21, "ETHCHF");

            Assert.AreEqual(13, traderService.getMaxVolumeToSellAlwaysWin(trades, balances, assetPair, price));
        }
    }
}