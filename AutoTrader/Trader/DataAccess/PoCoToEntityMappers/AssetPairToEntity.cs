using System;

namespace AutoTrader.Data
{
    public class AssetPairToEntity
    {
        public AssetPairEntity create(AssetPair item)
        {

            if (item is not null)
            {
                return new AssetPairEntity(item.Id, item.Name, item.Accuracy, item.BaseAssetId, item.QuotingAssetId);
            }

            throw new ArgumentException();
        }

        public AssetPair create(AssetPairEntity item)
        {

            if (item is not null)
            {
                return new AssetPair(item.Id, item.Name, item.Accuracy, item.BaseAssetId, item.QuotingAssetId);
            }

            throw new ArgumentException();
        }

    }
}
