using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Trader.PoCos;

namespace AutoTrader.Repository
{
    public interface IRepository
    {
        Task<Dictionary<String, AssetPair>> GetAssetPairs();

        Task<Boolean> IsAliveAsync();

        Task<IPrice> GetHistoryRatePerDay(String assetPairId, DateTime date);

        Task<List<IBalance>> GetWallets();

        Task<List<TradeEntry>> GetTrades();

        Task<IPrice> GetPrice(String assetPairId);

        Task<IResponse<String>> LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        Task<IResponse<String>> LimitOrderSell(String assetPairId, Decimal price, Decimal volume);

        Task<List<Operation>> GetOperations();

    }

    public interface IRepositoryGen<T>
    {
        T GetAssetPairsDictionary();

        T GetAssetPairs();

        T IsAliveAsync();

        T GetHistoryRatePerDay(String assetPairId, DateTime date);

        T GetWallets();

        T GetTrades();

        T GetPrice(String assetPairId);

        T LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        T LimitOrderSell(String assetPairId, Decimal price, Decimal volume);

        T GetOperations();
    }

}
