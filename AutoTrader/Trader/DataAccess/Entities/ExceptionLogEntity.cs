using System;

namespace AutoTrader.Data
{
    /// Private setters are only for EntityFramework Core. Without any Setter, the EntityFramework would not recognise it as a field (Even when they are part of the constructor)
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