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
    public class RetryRepositoryTests
    {
        IRepository repository = new RetryRepository(
            new NullLogger<RetryRepository>(),
            new RawResponseRepository(new NullLogger<RawResponseRepository>(), 
                new LykkeRepository(new NullLogger<LykkeRepository>(), new TraderConfig { apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InRyYWRpbmctd2FsbGV0LTEiLCJhdWQiOiJoZnQtYXBpdjIubHlra2UuY29tIiwia2V5LWlkIjoiYjc1NWYzMzctNzFlNy00N2VmLWJmNGUtOTgyMmI3ZTkwYTdlIiwiY2xpZW50LWlkIjoiNDg3YzQxOWYtMGI2YS00MzIyLWFkYzctZjRmOTc5NjA1YTA4Iiwid2FsbGV0LWlkIjoiODdjNDIyNmQtMWRhYi00NjM2LTgwNDMtN2I2MzhkM2I4NzAwIiwibmJmIjoxNTkzMzQ2MjgzLCJleHAiOjE5MDg4NzkwODMsImlhdCI6MTU5MzM0NjI4M30.FCCkW9qTaUUPP6pFVIPunNR-clEgN9k2gZB0ZtHidtk" }))
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

        [TestMethod()]
        public async Task GetWalletTest()
        {
            List<IWalletEntry> task = await repository.GetWallets();
            Assert.IsTrue(task.Count > 1);
        }
    }
}