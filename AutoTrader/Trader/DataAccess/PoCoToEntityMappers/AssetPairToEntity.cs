using System;
using AutoTrader.Models;

namespace AutoTrader.Data
{
    public class AssetPairToEntity
    {
        public AssetPairEntity mapTo(AssetPair item)
        {

            if (item is not null)
            {
                return new AssetPairEntity(item.Id, item.Name, item.PriceAccuracy, item.BaseAssetId, item.QuotingAssetId, item.BaseAssetAccuracy, item.QuoteAssetAccuracy);
            }

            throw new ArgumentException();
        }

        public AssetPair mapTo(AssetPairEntity item)
        {

            if (item is not null)
            {
                return new AssetPair(item.Id, item.Name, item.PriceAccuracy, item.BaseAssetId, item.QuotingAssetId, item.PriceAccuracy, item.QuotingAssetAccuracy, item.BaseAssetAccuracy);
            }

            throw new ArgumentException();
        }

    }
}
