using System;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class PayloadPrice
    {
        public string assetPairId { get; set; }
        public float bid { get; set; }
        public float ask { get; set; }

        [JsonConverter(typeof(UnixMillisecondsConverter))]
        public DateTime timestamp { get; set; }
    }
}