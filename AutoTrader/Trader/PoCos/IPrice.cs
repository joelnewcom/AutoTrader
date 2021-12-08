using System;

namespace AutoTrader.Data
{
    public interface IPrice
    {
        DateTime Date { get; set; }
        float Ask { get; set; }
        float Bid { get; set; }
    }
}