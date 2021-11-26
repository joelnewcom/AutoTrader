using System;

namespace AutoTrader.Data
{
    public class Price : IPrice
    {
        public DateTime Date { get; set; }

        public float Ask { get; set; }

        public float Bid { get; set; }

        public Price(DateTime date, float ask, float bid)
        {
            Date = date;
            Ask = ask;
            Bid = bid;
        }
    }
}