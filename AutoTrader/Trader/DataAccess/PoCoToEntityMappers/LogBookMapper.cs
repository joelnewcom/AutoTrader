using AutoTrader.Data;

namespace AutoTrader.Trader.DataAccess.PoCoToEntityMappers
{
    public class LogBookMapper
    {
        public LogBookEntity create(LogBook poco)
        {
            return new LogBookEntity(poco.Id, poco.AssetPairId, poco.Date, poco.logBookEntry, poco.reason);
        }

        public LogBook create(LogBookEntity entity)
        {
            return new LogBook(entity.Id, entity.AssetPairId, entity.Date, entity.logBookEntry, entity.reason);
        }
    }
}