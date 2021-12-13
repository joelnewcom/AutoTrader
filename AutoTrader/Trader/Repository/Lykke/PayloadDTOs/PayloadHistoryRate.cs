using System;

namespace AutoTrader.Repository
{
    public class PayloadHistoryRate
    {
        public string Id { get; set; }
        public Decimal Bid { get; set; }
        public Decimal Ask { get; set; }
        public Decimal TradingVolume { get; set; }
        public Decimal TradingOppositeVolume { get; set; }
    }

}