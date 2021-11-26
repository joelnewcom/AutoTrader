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

        public async Task<IPrice> RefreshAssetPairHistory(String assetPairId)
        {
            DateTime lastDayWeAlreadyHave = data.GetDateOfLatestEntry(assetPairId);
            IPrice historyRatePerDay = new NoDataPrice();
            while (lastDayWeAlreadyHave < DateTime.Today)
            {
                lastDayWeAlreadyHave = lastDayWeAlreadyHave.AddDays(1);
                historyRatePerDay =  await repository.GetHistoryRatePerDay(assetPairId, lastDayWeAlreadyHave);
                if (historyRatePerDay is Price)
                {
                    data.AddAssetPairHistoryEntry(assetPairId, (Price) historyRatePerDay);
                }
                await Task.Delay(secondsToWaitForNextRequest * 1000);
            }
            return historyRatePerDay;
        }
    }
}