using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;

namespace AutoTrader.Repository
{
    public interface IRepository
    {
        Task<Dictionary<String, AssetPair>> GetAssetPairsDictionary();

        Task<Boolean> IsAliveAsync();

        Task<IAssetPairHistoryEntry> GetHistoryRatePerDay(String assetPairId, DateTime date);

        Task<List<IWalletEntry>> GetWallets();
    }

    public interface IRepositoryGen<T>
    {
        T GetAssetPairsDictionary();

        T IsAliveAsync();

        T GetHistoryRatePerDay(String assetPairId, DateTime date);

        T GetWallets();
    }

}
