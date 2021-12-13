using System;

namespace AutoTrader.Data
{
    public class PriceEntity
    {
        /// Primary Key
        public Guid Id { get; private set;}
        public String AssetPairId { get; private set; }
        public DateTime Date { get; private set; }

        public Decimal Ask { get; private set; }

        public Decimal Bid { get; private set; }

        public PriceEntity(DateTime date, Decimal ask, Decimal bid, string assetPairId)
        {
            Id = Guid.NewGuid();
            Date = date;
            Ask = ask;
            Bid = bid;
            AssetPairId = assetPairId;
        }
    }
}