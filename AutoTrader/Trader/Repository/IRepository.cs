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

        Task<IPrice> GetHistoryRatePerDay(String assetPairId, DateTime date);

        Task<List<IBalance>> GetWallets();

        Task<List<TradeEntry>> GetTrades();

        Task<IPrice> GetPrice(String assetPairId);

        Task<String> LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        Task<String> LimitOrderSell(String assetPairId, Decimal price, Decimal volume);
    }

    public interface IRepositoryGen<T>
    {
        T GetAssetPairsDictionary();

        T IsAliveAsync();

        T GetHistoryRatePerDay(String assetPairId, DateTime date);

        T GetWallets();

        T GetTrades();

        T GetPrice(String assetPairId);

        T LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        T LimitOrderSell(String assetPairId, Decimal price, Decimal volume);
    }

}
