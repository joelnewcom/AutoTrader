using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Repository
{
    public interface IRepository
    {
        Task<Dictionary<String, AssetPair>> GetAssetPairsDictionary();

        Task<Boolean> IsAliveAsync();

        Task<IAssetPairHistoryEntry> GetHistoryRatePerDay(AssetPair assetPair, DateTime date);
    }

    public interface IRepositoryGen<T>
    {
        T GetAssetPairsDictionary();

        T IsAliveAsync();

        T GetHistoryRatePerDay(AssetPair assetPair, DateTime date);
    }

}
