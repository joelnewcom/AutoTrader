using AutoTrader.Repository;
using System;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Library
{
    public class DataRefresher
    {
        private readonly IRepository repository;
        private readonly IDataAccessAsync data;
        private int secondsToWaitForNextRequest = 10;

        public DataRefresher(IRepository repository, IDataAccessAsync data)
        {
            this.repository = repository;
            this.data = data;
        }

        public async Task<IPrice> RefreshAssetPairHistory(String assetPairId)
        {
            DateTime lastDayWeAlreadyHave = await data.GetDateOfLatestEntry(assetPairId);
            IPrice historyRatePerDay = new NoDataPrice();
            while (lastDayWeAlreadyHave < DateTime.Today)
            {
                lastDayWeAlreadyHave = lastDayWeAlreadyHave.AddDays(1);
                historyRatePerDay =  await repository.GetHistoryRatePerDay(assetPairId, lastDayWeAlreadyHave);
                if (historyRatePerDay is Price)
                {
                    await data.AddAssetPairHistoryEntry(assetPairId, (Price) historyRatePerDay);
                }
                await Task.Delay(secondsToWaitForNextRequest * 1000);
            }
            return historyRatePerDay;
        }
    }
}