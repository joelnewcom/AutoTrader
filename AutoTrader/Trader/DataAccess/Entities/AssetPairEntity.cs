using System;

namespace AutoTrader.Data
{
    /// Private setters are only for EntityFramework Core. Without any Setter, the EntityFramework would not recognise it as a field (Even when they are part of the constructor)
    public class AssetPairEntity
    {
        /// Primary Key
        public String Id { get; private set; }

        public String Name { get; private set; }

        public int PriceAccuracy { get; private set; }

        public String BaseAssetId { get; private set; }

        public String QuotingAssetId { get; private set; }
        
        public int BaseAssetAccuracy { get; private set; }

        public int QuotingAssetAccuracy { get; private set; }

        public AssetPairEntity(String id, String name, int priceAccuracy, String baseAssetId, String quotingAssetId, int baseAssetAccuracy, int quotingAssetAccuracy)
        {
            this.Id = id;
            this.Name = name;
            this.PriceAccuracy = priceAccuracy;
            this.BaseAssetId = baseAssetId;
            this.QuotingAssetId = quotingAssetId;
            this.BaseAssetAccuracy = baseAssetAccuracy;
            this.QuotingAssetAccuracy = quotingAssetAccuracy;
        }
    }
}