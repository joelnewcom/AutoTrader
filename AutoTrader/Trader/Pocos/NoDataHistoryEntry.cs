using System;

namespace AutoTrader.Data
{
    public class NoDataHistoryEntry : IAssetPairHistoryEntry
    {
        public DateTime Date
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public float Ask
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public float Buy
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}