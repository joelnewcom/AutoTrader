using System;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoTraderTests.Trader.Data
{
    public class DataInMemoryTests
    {
        readonly AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000, "CHF", "ETH",0,0,0);

        [Fact]
        public async Task NewestDataAvailableGetReturned()
        {
            // given
            DateTime today = DateTime.Today;
            DataInMemory dataAccess = new DataInMemory(new NullLogger<DataInMemory>());

            Price firstAssetPairHistoryEntry = new Price(DateTime.Today.AddDays(-2), 10, 10, "ETHCHF");
            Price secondAssetPairHistoryEntry = new Price(DateTime.Today.AddDays(-1), 10, 10, "ETHCHF");
            Price thirdAssetPairHistoryEntry = new Price(today, 10, 10, "ETHCHF");

            await dataAccess.AddAssetPairHistoryEntry("eth/chf", firstAssetPairHistoryEntry);
            await dataAccess.AddAssetPairHistoryEntry("eth/chf", secondAssetPairHistoryEntry);
            await dataAccess.AddAssetPairHistoryEntry("eth/chf", thirdAssetPairHistoryEntry);

            // when
            DateTime resultToBeVerified = await dataAccess.GetDateOfLatestEntry("eth/chf");

            // then
            Assert.AreEqual(today, resultToBeVerified);
        }
    }
}