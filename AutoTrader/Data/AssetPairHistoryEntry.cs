using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public class AssetPairHistoryEntry : IAssetPairHistoryEntry
    {
        public DateTime Date { get; set; }

        public float Ask { get; set; }

        public float Buy { get; set; }

        public AssetPairHistoryEntry(DateTime date, float ask, float buy)
        {
            Date = date;
            Ask = ask;
            Buy = buy;
        }
    }
}