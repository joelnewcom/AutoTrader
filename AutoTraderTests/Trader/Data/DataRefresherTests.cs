using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Library;
using System;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;
using System.Threading.Tasks;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class DataRefresherTests
    {
        readonly Mock<IRepository> repoMock = new Mock<IRepository>();
        readonly Mock<IDataAccessAsync> dataAccessMock = new Mock<IDataAccessAsync>();
        readonly AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000, "CHF", "ETH", 0, 0, 0);

        [TestMethod()]
        public async Task EntryOfTodayIsNotAdded()
        {
            // given
            Price assetPairHistoryEntry = new Price(DateTime.Today, 10, 10, "ETHCHF");
            dataAccessMock.Setup(p => p.GetDateOfLatestEntry(assetPair.Id)).Returns(Task.FromResult(DateTime.Today.AddDays(-1)));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<String>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);
            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            await dataRefresher.RefreshAssetPairHistory(assetPair.Id);

            // then
            dataAccessMock.Verify(s => s.AddAssetPairHistoryEntry(assetPair.Id, assetPairHistoryEntry), Times.Never);
        }

        [TestMethod()]
        public async Task NewEntryGetsAdded()
        {
            // given
            Price assetPairHistoryEntry = new Price(DateTime.Today, 10, 10, "ETHCHF");
            dataAccessMock.Setup(p => p.GetDateOfLatestEntry(assetPair.Id)).Returns(Task.FromResult(DateTime.Today.AddDays(-2)));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<String>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);
            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            await dataRefresher.RefreshAssetPairHistory(assetPair.Id);

            // then
            dataAccessMock.Verify(s => s.AddAssetPairHistoryEntry(assetPair.Id, assetPairHistoryEntry), Times.Once);
        }

        [TestMethod()]
        public async Task OldEntryGetsOmitted()
        {
            // given
            DateTime dateTime = DateTime.Today;
            Price assetPairHistoryEntry = new Price(dateTime, 10, 10, "ETHCHF");

            dataAccessMock.Setup(p => p.GetDateOfLatestEntry(assetPair.Id)).Returns(Task.FromResult(dateTime));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<String>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);

            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            await dataRefresher.RefreshAssetPairHistory(assetPair.Id);

            // then
            dataAccessMock.Verify(
                s => s.AddAssetPairHistoryEntry(It.IsAny<String>(), It.IsAny<Price>()),
                Times.Never);
        }

        [TestMethod()]
        public async Task MultipleEntriesGetAdded()
        {
            // given
            DateTime today = DateTime.Today;
            Price assetPairHistoryEntry = new Price(today, 10, 10, "ETHCHF");

            dataAccessMock.Setup(p => p.GetDateOfLatestEntry(assetPair.Id)).Returns(Task.FromResult(today.AddDays(-3)));
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<String>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);

            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            await dataRefresher.RefreshAssetPairHistory(assetPair.Id);

            // then
            dataAccessMock.Verify(
                s => s.AddAssetPairHistoryEntry(It.IsAny<String>(), It.IsAny<Price>()),
                Times.Exactly(2));
        }
    }
}