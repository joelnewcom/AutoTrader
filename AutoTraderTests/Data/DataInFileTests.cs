﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class DataInFileTests
    {
        readonly AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000, "CHF", "ETH");

        [TestMethod()]
        public void NewestDataAvailableGetReturned()
        {
            // given
            DataInFile dataInFile = new DataInFile(new NullLogger<DataInFile>());
            DateTime today = DateTime.Today;

            Price firstAssetPairHistoryEntry = new Price(DateTime.Today.AddDays(-2), 10, 10);
            Price secondAssetPairHistoryEntry = new Price(DateTime.Today.AddDays(-1), 10, 10);
            Price thirdAssetPairHistoryEntry = new Price(today, 10, 10);

            dataInFile.AddAssetPairHistoryEntry("eth/chf", firstAssetPairHistoryEntry);
            dataInFile.AddAssetPairHistoryEntry("eth/chf", secondAssetPairHistoryEntry);
            dataInFile.AddAssetPairHistoryEntry("eth/chf", thirdAssetPairHistoryEntry);

            // when
            DateTime resultToBeVerified = dataInFile.GetDateOfLatestEntry("eth/chf");

            // then
            Assert.AreEqual(today, resultToBeVerified);
        }
    }
}