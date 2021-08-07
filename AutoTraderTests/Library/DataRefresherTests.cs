using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Library;
using System;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class DataRefresherTests
    {
        readonly Mock<IRepository> repoMock = new Mock<IRepository>();
        readonly Mock<IDataAccess> dataAccessMock = new Mock<IDataAccess>();
        readonly AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000);

        [TestMethod()]
        public void NewEntryGetsAdded()
        {
            // given
            IAssetPairHistoryEntry assetPairHistoryEntry = new AssetPairHistoryEntry(DateTime.Today, 10, 10);
            dataAccessMock.Setup(p => p.GetYoungestDate(assetPair)).Returns(DateTime.Today.AddDays(-1));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<AssetPair>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);
            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            dataRefresher.RefreshAssetPairHistory(assetPair);

            // then
            dataAccessMock.Verify(s => s.AddAssetPairHistoryEntry(assetPair, assetPairHistoryEntry), Times.Once);
        }

        [TestMethod()]
        public void OldEntryGetsOmitted()
        {
            // given
            DateTime dateTime = DateTime.Today;
            IAssetPairHistoryEntry assetPairHistoryEntry = new AssetPairHistoryEntry(dateTime, 10, 10);

            dataAccessMock.Setup(p => p.GetYoungestDate(assetPair)).Returns(dateTime);
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<AssetPair>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);

            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            dataRefresher.RefreshAssetPairHistory(assetPair);

            // then
            dataAccessMock.Verify(
                s => s.AddAssetPairHistoryEntry(It.IsAny<AssetPair>(), It.IsAny<IAssetPairHistoryEntry>()),
                Times.Never);
        }

        [TestMethod()]
        public void MultipleEntriesGetAdded()
        {
            // given
            DateTime dateTime = DateTime.Today;
            IAssetPairHistoryEntry assetPairHistoryEntry = new AssetPairHistoryEntry(dateTime, 10, 10);

            dataAccessMock.Setup(p => p.GetYoungestDate(assetPair)).Returns(dateTime.AddDays(-2));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<AssetPair>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);

            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            dataRefresher.RefreshAssetPairHistory(assetPair);

            // then
            dataAccessMock.Verify(
                s => s.AddAssetPairHistoryEntry(It.IsAny<AssetPair>(), It.IsAny<IAssetPairHistoryEntry>()),
                Times.Exactly(2));
        }
    }
}