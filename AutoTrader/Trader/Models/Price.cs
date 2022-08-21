using System;

namespace AutoTrader.Models
{
    public class Price : IPrice
    {
        public String AssetPairId { get; set; }
        public DateTime Date { get; set; }

        public Decimal Ask { get; set; }

        public Decimal Bid { get; set; }

        public Price(DateTime date, Decimal ask, Decimal bid, string assetPairId)
        {
            Date = date;
            Ask = ask;
            Bid = bid;
            AssetPairId = assetPairId;
        }
    }
}