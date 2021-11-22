using System;

namespace AutoTrader.Data
{
    public class AssetPairHistoryEntry : IAssetPairHistoryEntry
    {
        public DateTime Date { get; set; }

        public float Ask { get; set; }

        public float Bid { get; set; }

        public AssetPairHistoryEntry(DateTime date, float ask, float bid)
        {
            Date = date;
            Ask = ask;
            Bid = bid;
        }
    }
}