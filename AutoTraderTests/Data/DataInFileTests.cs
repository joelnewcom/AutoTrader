using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class DataInFileTests
    {
        readonly Mock<IRepository> repoMock = new Mock<IRepository>();
        readonly Mock<IDataAccess> dataAccessMock = new Mock<IDataAccess>();
        readonly AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000);

        [TestMethod()]
        public void NewestDataAvailableGetReturned()
        {
            // given
            DataInFile dataInFile = new DataInFile();
            DateTime today = DateTime.Today;

            AssetPairHistoryEntry firstAssetPairHistoryEntry = new AssetPairHistoryEntry(DateTime.Today.AddDays(-2), 10, 10);
            AssetPairHistoryEntry secondAssetPairHistoryEntry = new AssetPairHistoryEntry(DateTime.Today.AddDays(-1), 10, 10);
            AssetPairHistoryEntry thirdAssetPairHistoryEntry = new AssetPairHistoryEntry(today, 10, 10);

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