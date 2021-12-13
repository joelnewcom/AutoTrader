using System;
namespace AutoTrader.Repository
{
    public class PayloadLimitOrder
    {
        public string AssetPairId { get; set; }
        public string Side { get; set; }

        public Decimal Volume { get; set; }
        public Decimal Price { get; set;}
    }
}
