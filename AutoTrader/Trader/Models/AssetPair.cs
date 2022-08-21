using System;

namespace AutoTrader.Models
{

    public class AssetPair
    {
        public String Id { get; }
        public String Name { get; private set; }
        public String BaseAssetId { get; private set; }
        public String QuotingAssetId { get; private set; }
        public int PriceAccuracy { get; private set; }
        public int QuoteAssetAccuracy { get; private set; }
        public int BaseAssetAccuracy { get; private set; }

        public AssetPair(String id, String name, int accuracy, String baseAssetId, String quotingAssetId, int priceAccurary, int quoteAssetAccuracy, int baseAssetAccuracy)
        {
            this.Id = id;
            this.Name = name;
            this.BaseAssetId = baseAssetId;
            this.QuotingAssetId = quotingAssetId;
            this.PriceAccuracy = priceAccurary;
            this.QuoteAssetAccuracy = quoteAssetAccuracy;
            this.BaseAssetAccuracy = baseAssetAccuracy;
        }
    }
}