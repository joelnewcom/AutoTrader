using AutoTrader.Trader.PoCos;

namespace AutoTrader.Data
{
    public class ExceptionLogMapper
    {
        public ExceptionLogEntity create(ExceptionLog item)
        {
            return new ExceptionLogEntity(item.Id, item.Message, item.DateTime);
        }

        public ExceptionLog create(ExceptionLogEntity item)
        {
            return new ExceptionLog(item.Id, item.Message, item.DateTime);
        }
    }
}
