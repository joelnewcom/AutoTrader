using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using AutoTrader.Trader.Repository.Lykke.PocoMapper;
using AutoTrader.Config;
using AutoTrader.Models;

namespace AutoTraderTests.Repository
{
    [TestClass()]
    public class RetryRepositoryTests
    {
        IRepository repository = new BusinessDomainRepository(
            new NullLogger<BusinessDomainRepository>(),
            new RawResponseRepository(new NullLogger<RawResponseRepository>(), 
                new LykkeRepository(new NullLogger<LykkeRepository>(), new TraderConfig { apiKey = "" })),
                new AssetPairHistoryEntryMapper(),
                new TradeEntryMapper(),
                new PriceMapper(),
                new AssetPairMapper(),
                new OperationMapper()
        );

        [TestMethod()]
        public async Task GetHistoryRatePerDayTest()
        {
            Task<IPrice> historyRatePerDay = repository.GetHistoryRatePerDay("BTCCHF", DateTime.Today);
            await historyRatePerDay;
            IPrice assetPairHistoryEntry = historyRatePerDay.Result;
            Assert.AreEqual(DateTime.Today, assetPairHistoryEntry.Date);
        }

        [TestMethod()]
        public async Task GetDictionaryTest()
        {
            Dictionary<string, AssetPair> task = await repository.GetAssetPairs();
            Assert.IsTrue(task.Count > 1);
        }

        [TestMethod()]
        public async Task GetWalletTest()
        {
            List<IBalance> walletList = await repository.GetWallets();
            Assert.IsTrue(walletList.Count == 0);
        }
    }
}