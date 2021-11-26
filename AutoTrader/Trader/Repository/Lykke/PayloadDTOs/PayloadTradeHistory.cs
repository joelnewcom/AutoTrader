using System;
using Newtonsoft.Json;

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
        public float price { get; set; }
        public float baseVolume { get; set; }
        public float quoteVolume { get; set; }
        public string baseAssetId { get; set; }
        public string quoteAssetId { get; set; }
        public PayloadFee fee { get; set; }
    }
}