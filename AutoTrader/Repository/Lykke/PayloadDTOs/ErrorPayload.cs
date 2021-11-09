using System.Collections.Generic;

namespace AutoTrader.Repository
{
    public class ErrorPayload
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Fields { get; set; }
    }
}