using System;

namespace AutoTrader.Trader.PoCos
{
    public class ExceptionLog
    {
        public String Message { get; private set; }

        public DateTime DateTime { get; private set; }

        public Guid Id { get; private set; }

        public ExceptionLog(Guid id, string message, DateTime dateTime)
        {
            Id = id;
            Message = message;
            DateTime = dateTime;
        }
    }
}