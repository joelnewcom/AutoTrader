using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace AutoTraderTests.Repository
{
    [TestClass()]
    public class LykkeRepositoryTests
    {
        IRepository repository = new RetryRepository(
            new NullLogger<RetryRepository>(),
            new RawResponseRepository(new NullLogger<RawResponseRepository>(), new LykkeRepository(new NullLogger<LykkeRepository>()))
        );

        [TestMethod()]
        public async Task GetHistoryRatePerDayTest()
        {
            Task<IAssetPairHistoryEntry> historyRatePerDay = repository.GetHistoryRatePerDay("BTCCHF", DateTime.Today);
            await historyRatePerDay;
            IAssetPairHistoryEntry assetPairHistoryEntry = historyRatePerDay.Result;
            Assert.AreEqual(DateTime.Today, assetPairHistoryEntry.Date);
        }

        [TestMethod()]
        public async Task GetDictionaryTest()
        {
            Dictionary<string, AssetPair> task = await repository.GetAssetPairsDictionary();
            Assert.IsTrue(task.Count > 1);
        }
    }
}