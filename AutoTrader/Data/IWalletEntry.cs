using System;

namespace AutoTrader.Data
{
    public interface IWalletEntry
    {
        String AssetId { get; set; }

        float Balance { get; set; }

        float Reserved { get; set; }
    }
}