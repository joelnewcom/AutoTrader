using System.Collections.Generic;

namespace AutoTrader.Config
{
    public class TraderConfig
    {
        public List<string> knownAssetPairIds { get; set; }
        public string apiKey { get; set; }

        public SafetyCatch safetyCatch { get; set; }
    }
}