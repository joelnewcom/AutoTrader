using System;
namespace AutoTrader.Repository
{
    public class PayloadPrice
    {
        public string assetPairId { get; set; }
        public float bid { get; set; }
        public float ask { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
