using System;
namespace AutoTrader.Repository
{
    public class PayloadLimitOrder
    {
        public string AssetPairId { get; set; }
        public string Side { get; set; }

        public float Volume { get; set; }
        public float Price { get; set;}
    }
}
