using System.Collections.Generic;

namespace AutoTrader.Config
{
    public class TraderConfig
    {
        public List<string> knownAssetPairIds { get; set; }
        public string apiKey { get; set; }

        // Needs to be virtual for testing purposes
        public virtual SafetyCatch safetyCatch { get; set; }
    }
}