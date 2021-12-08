using System;

namespace AutoTrader.Data
{
    public class PriceEntity
    {
        /// Primary Key
        public Guid Id { get; private set;}
        public String AssetPairId { get; private set; }
        public DateTime Date { get; private set; }

        public float Ask { get; private set; }

        public float Bid { get; private set; }

        public PriceEntity(DateTime date, float ask, float bid, string assetPairId)
        {
            Id = Guid.NewGuid();
            Date = date;
            Ask = ask;
            Bid = bid;
            AssetPairId = assetPairId;
        }
    }
}