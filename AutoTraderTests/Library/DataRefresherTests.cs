using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Repository;
using Moq;

namespace AutoTraderTests.Library
{
    [TestClass()]
    public class DataRefresherTests
    {
        [TestMethod()]
        public void RefreshAssetPairHistoryTest()
        {
            // given
            Mock<IRepository> repoMock = new Mock<IRepository>();
            Mock<IDataAccess> dataAccessMock = new Mock<IDataAccess>();

            AssetPair assetPair = new AssetPair("eth/chf", "testAssetPair", 1000);
            IAssetPairHistoryEntry assetPairHistoryEntry = new AssetPairHistoryEntry(DateTime.Today, 10, 10);
            repoMock.Setup(p => p.GetHistoryRatePerDay(It.IsAny<AssetPair>(), It.IsAny<DateTime>()))
                .ReturnsAsync(assetPairHistoryEntry);

            DataRefresher dataRefresher = new DataRefresher(repoMock.Object, dataAccessMock.Object);

            // when
            dataRefresher.RefreshAssetPairHistory(assetPair);

            // then
            dataAccessMock.Verify(s => s.AddAssetPairHistoryEntry(assetPair, assetPairHistoryEntry));
        }
    }
}