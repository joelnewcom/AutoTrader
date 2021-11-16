using System;

namespace AutoTrader.Data
{
    public class TradeEntry
    {
        public string Id { get; set; }
        public DateTime timestamp { get; set; }
        public string assetPairId { get; set; }
        public string price { get; set; }
        public string baseAssetId { get; set; }
        public string quoteAssetId { get; set; }

        public TradeEntry(string id, DateTime timestamp, string assetPairId, string price, string baseAssetId, string quoteAssetId)
        {
            Id = id;
            this.timestamp = timestamp;
            this.assetPairId = assetPairId;
            this.price = price;
            this.baseAssetId = baseAssetId;
            this.quoteAssetId = quoteAssetId;
        }
    }
}