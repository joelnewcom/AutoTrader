using System;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class PayloadPrice
    {
        public string assetPairId { get; set; }
        public Decimal bid { get; set; }
        public Decimal ask { get; set; }

        [JsonConverter(typeof(UnixMillisecondsConverter))]
        public DateTime timestamp { get; set; }
    }
}