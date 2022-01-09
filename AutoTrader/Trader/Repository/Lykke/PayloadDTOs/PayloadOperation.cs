using System;
using Newtonsoft.Json;

namespace AutoTrader.Trader.Repository
{
    public class PayloadOperation
    {
        public String OperationId { get; set; }
        public String AssetId { get; set; }
        public Decimal TotalVolume { get; set; }
        public Decimal Fee { get; set; }
        public String Type { get; set; }
        [JsonConverter(typeof(UnixMillisecondsConverter))]
        public DateTime Timestamp { get; set; }
    }
}