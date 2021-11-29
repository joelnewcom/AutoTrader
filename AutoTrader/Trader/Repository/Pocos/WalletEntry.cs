using System;

namespace AutoTrader.Data
{
    public class Balance : IBalance
    {
        public String AssetId { get; set; }

        public float Available { get; set; }

        public float Reserved { get; set; }

        public Balance(String assetId, float balance, float reserved)
        {
            AssetId = assetId;
            Available = balance;
            Reserved = reserved;
        }
    }
}