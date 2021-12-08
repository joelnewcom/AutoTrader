using System;

namespace AutoTrader.Data
{
    /// Private setters are only for EntityFramework Core. Without any Setter, the EntityFramework would not recognise it as a field (Even when they are part of the constructor)
    public class AssetPairEntity
    {
        /// Primary Key
        public String Id { get; private set; }

        public String Name { get; private set; }

        public int Accuracy { get; private set; }

        public String BaseAssetId { get; private set; }

        public String QuotingAssetId { get; private set; }

        public AssetPairEntity(String id, String name, int accuracy, String baseAssetId, String quotingAssetId)
        {
            this.Id = id;
            this.Name = name;
            this.Accuracy = accuracy;
            this.BaseAssetId = baseAssetId;
            this.QuotingAssetId = quotingAssetId;
        }
    }
}