using AutoTrader.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Library
{
    public class DataRefresher
    {
        private readonly IRepository repository;
        private readonly IDataAccess data;

        public DataRefresher(IRepository repository, IDataAccess data)
        {
            this.repository = repository;
            this.data = data;
        }

        public async void RefreshAssetPairHistory(AssetPair assetPair)
        {
            DateTime nextDay = data.GetYoungestDate(assetPair);
            while (nextDay < DateTime.Today)
            {
                nextDay = nextDay.AddDays(1);
                Task<IAssetPairHistoryEntry> historyRatePerDay = repository.GetHistoryRatePerDay(assetPair, nextDay);
                if (await historyRatePerDay is not NoDataHistoryEntry)
                {
                    data.AddAssetPairHistoryEntry(assetPair, await historyRatePerDay);
                }
            }
        }
    }
}