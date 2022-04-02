using System;

namespace AutoTrader.Data
{
    public class BackendInfo
    {
        public String version { get; private set; }

        public BackendInfo(string version)
        {
            this.version = version;
        }
    }
}