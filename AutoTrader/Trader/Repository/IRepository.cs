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

        /// <param name="volume">Order volume (in base asset)</param>
        /// <param name="price">Order price(in quote asset for one unit of base asset)</param>
        Task<IResponse<String>> LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        /// <param name="volume">Order volume (in base asset)</param>
        /// <param name="price">Order price(in quote asset for one unit of base asset)</param>
        Task<IResponse<String>> LimitOrderSell(String assetPairId, Decimal price, Decimal volume);

        Task<List<Operation>> GetOperations();

        Task<List<Price>> GetPrices();
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

        /// <param name="volume">Order volume (in base asset)</param>
        /// <param name="price">Order price(in quote asset for one unit of base asset)</param>
        T LimitOrderBuy(String assetPairId, Decimal price, Decimal volume);

        /// <param name="volume">Order volume (in base asset)</param>
        /// <param name="price">Order price(in quote asset for one unit of base asset)</param>
        T LimitOrderSell(String assetPairId, Decimal price, Decimal volume);

        T GetOperations();

        T GetPrices();
    }

}
