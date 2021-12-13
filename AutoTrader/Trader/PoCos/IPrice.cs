using System;

namespace AutoTrader.Data
{
    public interface IPrice
    {
        DateTime Date { get; set; }
        Decimal Ask { get; set; }
        Decimal Bid { get; set; }
    }
}