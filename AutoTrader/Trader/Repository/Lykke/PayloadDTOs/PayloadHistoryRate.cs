using System;

namespace AutoTrader.Repository
{
    public class PayloadHistoryRate
    {
        public string Id { get; set; }
        public float Bid { get; set; }
        public float Ask { get; set; }
        public float TradingVolume { get; set; }
        public float TradingOppositeVolume { get; set; }
    }

}