using System;

namespace AutoTrader.Data
{
    public class WalletEntry : IWalletEntry
    {
        public String AssetId { get; set; }

        public float Balance { get; set; }

        public float Reserved { get; set; }

        public WalletEntry(String assetId, float balance, float reserved)
        {
            AssetId = assetId;
            Balance = balance;
            Reserved = reserved;
        }
    }
}