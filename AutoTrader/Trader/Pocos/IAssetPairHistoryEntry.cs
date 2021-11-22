using System;

namespace AutoTrader.Data
{
    public interface IAssetPairHistoryEntry
    {
        DateTime Date { get; set; }
        float Ask { get; set; }
        float Bid { get; set; }
    }
}