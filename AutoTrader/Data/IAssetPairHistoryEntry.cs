using System;

namespace AutoTrader.Data
{
    public interface IAssetPairHistoryEntry
    {
        DateTime Date { get; set; }
        float Ask { get; set; }
        float Buy { get; set; }
    }
}