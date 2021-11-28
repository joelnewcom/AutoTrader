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
                return new AssetPair(item.id, item.name, item.accuracy, item.baseAssetId, item.quotingAssetId);
            }

            throw new ArgumentException();
        }

    }
}
