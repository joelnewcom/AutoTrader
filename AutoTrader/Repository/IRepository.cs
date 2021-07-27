using AutoTrader.Library;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
