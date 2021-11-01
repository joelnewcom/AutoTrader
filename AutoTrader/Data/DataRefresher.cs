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

        public async void RefreshAssetPairHistory(String assetPairId)
        {
            DateTime lastDayWeAlreadyHave = data.GetDateOfLatestEntry(assetPairId);
            while (lastDayWeAlreadyHave < DateTime.Today)
            {
                lastDayWeAlreadyHave = lastDayWeAlreadyHave.AddDays(1);
                Task<IAssetPairHistoryEntry> historyRatePerDay = repository.GetHistoryRatePerDay(assetPairId, lastDayWeAlreadyHave);
                if (await historyRatePerDay is AssetPairHistoryEntry)
                {
                    data.AddAssetPairHistoryEntry(assetPairId, (AssetPairHistoryEntry) await historyRatePerDay);
                }
                await Task.Delay(secondsToWaitForNextRequest * 1000);
            }
        }
    }
}