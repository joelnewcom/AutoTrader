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

        public Decimal Ask
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public Decimal Bid
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
    }
}