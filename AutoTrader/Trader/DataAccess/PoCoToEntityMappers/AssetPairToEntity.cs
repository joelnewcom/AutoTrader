using System;

namespace AutoTrader.Data
{
    public class AssetPairToEntity
    {
        public AssetPairEntity create(AssetPair item)
        {

            if (item is not null)
            {
                return new AssetPairEntity(item.Id, item.Name, item.PriceAccuracy, item.BaseAssetId, item.QuotingAssetId, item.BaseAssetAccuracy, item.QuoteAssetAccuracy);
            }

            throw new ArgumentException();
        }

        public AssetPair create(AssetPairEntity item)
        {

            if (item is not null)
            {
                return new AssetPair(item.Id, item.Name, item.PriceAccuracy, item.BaseAssetId, item.QuotingAssetId, item.PriceAccuracy, item.QuotingAssetAccuracy, item.BaseAssetAccuracy);
            }

            throw new ArgumentException();
        }

    }
}
