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
        public Decimal price { get; }
        public string baseAssetId { get; }
        public Decimal baseVolume { get; }
        public string quoteAssetId { get; }
        public Decimal quoteVolume { get; }
        public TradeFee fee { get; }

        public TradeEntry(string id, DateTime timestamp, string assetPairId, Decimal price, string baseAssetId, string quoteAssetId, string role, string side, Decimal baseVolume, Decimal quoteVolume, TradeFee fee)
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