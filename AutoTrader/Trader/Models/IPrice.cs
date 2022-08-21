using System;

namespace AutoTrader.Models
{
    public interface IPrice
    {
        DateTime Date { get; set; }
        Decimal Ask { get; set; }
        Decimal Bid { get; set; }
    }
}