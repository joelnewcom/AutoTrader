using System;

namespace AutoTrader.Data
{
    public class Price : IPrice
    {
        public String AssetPairId { get; set; }
        public DateTime Date { get; set; }

        public float Ask { get; set; }

        public float Bid { get; set; }

        public Price(DateTime date, float ask, float bid, string assetPairId)
        {
            Date = date;
            Ask = ask;
            Bid = bid;
            AssetPairId = assetPairId;
        }
    }
}