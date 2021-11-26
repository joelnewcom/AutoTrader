using System;

namespace AutoTrader.Data
{
    public class NoDataPrice : IPrice
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

        public float Bid
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}