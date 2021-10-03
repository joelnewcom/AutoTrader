using System;

namespace AutoTrader.Repository
{
    public class PayloadAssetPairDict
    {
        public string id { get; set; }
        public string name { get; set; }

        public int accuracy { get; set; }

        public int invertedAccuracy { get; set; }

        public string baseAssetId { get; set; }

        public string quotingAssetId { get; set; }
    }
}