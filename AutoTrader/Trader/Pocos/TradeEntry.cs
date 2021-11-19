using System;

namespace AutoTrader.Data
{
    public class TradeEntry
    {
        public string id { get; }
        public DateTime timestamp { get; }
        public string assetPairId { get; }
        public string role { get; }
        public string side { get; }
        public string price { get; }
        public string baseAssetId { get; }
        public string baseVolume { get; }
        public string quoteAssetId { get; }
        public string quoteVolume { get; }
        public TradeFee fee { get; }

        public TradeEntry(string id, DateTime timestamp, string assetPairId, string price, string baseAssetId, string quoteAssetId, string role, string side, string baseVolume, string quoteVolume, TradeFee fee)
        {
            this.id = id;
            this.timestamp = timestamp;
            this.assetPairId = assetPairId;
            this.price = price;
            this.baseAssetId = baseAssetId;
            this.quoteAssetId = quoteAssetId;
            this.role = role;
            this.side = side;
            this.baseVolume = baseVolume;
            this.quoteVolume = quoteVolume;
            this.fee = fee;
        }
    }
}