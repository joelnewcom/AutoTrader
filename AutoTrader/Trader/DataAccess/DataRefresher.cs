using AutoTrader.Repository;
using System;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Models;

namespace AutoTrader.Library
{
    public class DataRefresher
    {
        private readonly IRepository _repository;
        private readonly IDataAccess _data;
        private int _secondsToWaitForNextRequest = 10;

        public DataRefresher(IRepository repository, IDataAccess data)
        {
            _repository = repository;
            _data = data;
        }

        public async Task<IPrice> RefreshAssetPairHistory(String assetPairId)
        {
            DateTime lastDayWeAlreadyHave = await _data.GetDateOfLatestEntry(assetPairId);
            IPrice historyRatePerDay = new NoDataPrice();
            DateTime yesterday = DateTime.Today.AddDays(-1);
            while (lastDayWeAlreadyHave < yesterday)
            {
                lastDayWeAlreadyHave = lastDayWeAlreadyHave.AddDays(1);
                historyRatePerDay = await _repository.GetHistoryRatePerDay(assetPairId, lastDayWeAlreadyHave);
                if (historyRatePerDay is Price)
                {
                    await _data.AddAssetPairHistoryEntry(assetPairId, (Price)historyRatePerDay);
                }
                await Task.Delay(_secondsToWaitForNextRequest * 1000);
            }
            return historyRatePerDay;
        }
    }
}