using System;

namespace AutoTrader.Data
{
    public class ExceptionLogEntity
    {
        public Guid Id { get; private set; }

        public String Message { get; private set; }

        public DateTime DateTime { get; private set; }

        public ExceptionLogEntity(Guid id, string message, DateTime dateTime)
        {
            Id = id;
            Message = message;
            DateTime = dateTime;
        }


    }
}