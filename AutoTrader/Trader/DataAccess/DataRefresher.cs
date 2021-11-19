using AutoTrader.Repository;
using System;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Library
{
    public class DataRefresher
    {
        private readonly IRepository repository;
        private readonly IDataAccess data;
        private int secondsToWaitForNextRequest = 10;

        public DataRefresher(IRepository repository, IDataAccess data)
        {
            this.repository = repository;
            this.data = data;
        }

        public async Task<IAssetPairHistoryEntry> RefreshAssetPairHistory(String assetPairId)
        {
            DateTime lastDayWeAlreadyHave = data.GetDateOfLatestEntry(assetPairId);
            IAssetPairHistoryEntry historyRatePerDay = new NoDataHistoryEntry();
            while (lastDayWeAlreadyHave < DateTime.Today)
            {
                lastDayWeAlreadyHave = lastDayWeAlreadyHave.AddDays(1);
                historyRatePerDay =  await repository.GetHistoryRatePerDay(assetPairId, lastDayWeAlreadyHave);
                if (historyRatePerDay is AssetPairHistoryEntry)
                {
                    data.AddAssetPairHistoryEntry(assetPairId, (AssetPairHistoryEntry) historyRatePerDay);
                }
                await Task.Delay(secondsToWaitForNextRequest * 1000);
            }
            return historyRatePerDay;
        }
    }
}