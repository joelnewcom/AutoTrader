using System;

namespace AutoTrader.Trader.PoCos
{
    public class ExceptionLog
    {
        public ExceptionLog(string message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }

        public String Message { get; set; }

        public DateTime DateTime { get; set; }

    }
}