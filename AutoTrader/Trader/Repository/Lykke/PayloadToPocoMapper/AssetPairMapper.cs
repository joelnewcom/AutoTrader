using System;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class AssetPairMapper
    {
        public AssetPair create(PayloadAssetPairDictEntry item)
        {
            if (item is not null)
            {
                return new AssetPair(item.id, item.name, item.accuracy, item.baseAssetId, item.quotingAssetId, 0, 0, 0);
            }

            throw new ArgumentException();
        }

        public AssetPair create(PayloadAssetPair item)
        {
            if (item is not null)
            {
                return new AssetPair(item.assetPairId, item.name, item.priceAccuracy, item.baseAssetId, item.quoteAssetId, item.priceAccuracy, item.quoteAssetAccuracy, item.baseAssetAccuracy);
            }

            throw new ArgumentException();
        }

    }
}
