using System;
namespace AutoTrader.Repository
{
    public class PayloadGetHistoryRate
    {
        public string Period { get; set; }
        public DateTime DateTime { get; set; }
    }
}
