using System;

namespace AutoTrader.Repository
{
    public class PayloadBalance
    {
        public String AssetId { get; set; }
        public Decimal Available { get; set; }
        public Decimal Reserved { get; set; }
        public float Timestamp { get; set; }
    }
}