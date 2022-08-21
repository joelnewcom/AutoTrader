using System;

namespace AutoTrader.Models
{
    public class Balance : IBalance
    {
        public String AssetId { get; set; }

        public Decimal Available { get; set; }

        public Decimal Reserved { get; set; }

        public Balance(String assetId, Decimal balance, Decimal reserved)
        {
            AssetId = assetId;
            Available = balance;
            Reserved = reserved;
        }
    }
}