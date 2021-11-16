using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AutoTrader.Repository
{
    public class PayloadTradeHistory
    {
        public string Id { get; set; }

        [JsonConverter(typeof(UnixMillisecondsConverter))]
        public DateTime timestamp { get; set; }

        public string assetPairId { get; set; }
        public string orderId { get; set; }
        public string role { get; set; }
        public string side { get; set; }
        public string price { get; set; }
        public string baseVolume { get; set; }
        public string quoteVolume { get; set; }
        public string baseAssetId { get; set; }
        public string quoteAssetId { get; set; }
        public PayloadFee fee { get; set; }
    }
}