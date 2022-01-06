using System;
using Microsoft.EntityFrameworkCore;

namespace AutoTrader.Data
{
    [Keyless]
    public class ExceptionLogEntity
    {
        public ExceptionLogEntity(string message, DateTime dateTime)
        {
            Message = message;
            DateTime = dateTime;
        }

        public String Message { get; set; }

        public DateTime DateTime { get; set; }

    }
}