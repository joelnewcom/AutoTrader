using System;

namespace AutoTrader.Repository
{
    public class PayloadBalance
    {
        public String AssetId { get; set; }
        public float Available { get; set; }
        public float Reserved { get; set; }
        public float Timestamp { get; set; }
    }
}