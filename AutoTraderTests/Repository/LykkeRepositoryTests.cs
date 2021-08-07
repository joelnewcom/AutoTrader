using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTraderTests.Repository
{
    [TestClass()]
    public class LykkeRepositoryTests
    {
        [TestMethod()]
        public async Task GetHistoryRatePerDayTest()
        {
            IRepository repository = new LykkeRepository();
            Task<IAssetPairHistoryEntry> historyRatePerDay =
                repository.GetHistoryRatePerDay(new AssetPair("BTCCHF", "BTC/CHF", 3), DateTime.Today);
            await historyRatePerDay;
            IAssetPairHistoryEntry assetPairHistoryEntry = historyRatePerDay.Result;
            Assert.AreEqual(DateTime.Today, assetPairHistoryEntry.Date);
        }
    }
}