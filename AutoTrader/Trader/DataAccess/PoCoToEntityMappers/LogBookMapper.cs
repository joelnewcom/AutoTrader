using System.Collections.Generic;
using AutoTrader.Data;
using AutoTrader.Models;
using AutoTrader.Trader.Advisor;

namespace AutoTrader.Trader.DataAccess.PoCoToEntityMappers
{
    public class LogBookMapper
    {
        private DecisionMapper decisionMapper = new DecisionMapper();
        //private FromEntityToPocoDecisionMapper decisionMapper = new FromEntityToPocoDecisionMapper();
        public LogBookEntity mapTo(LogBook poco)
        {
            LogBookEntity logBookEntity = new LogBookEntity(poco.Id, poco.AssetPairId, poco.Date, poco.reason);
            logBookEntity.Decisions = mapTo(poco.decisions);
            return logBookEntity;
        }

        internal List<DecisionEntity> mapTo(List<Decision> decisions)
        {
            List<DecisionEntity> mappedDecisions = new List<DecisionEntity>();
            decisions.ForEach(element => mappedDecisions.Add(decisionMapper.mapTo(element)));
            return mappedDecisions;
        }

        public LogBook mapTo(LogBookEntity entity)
        {
            return new LogBook(entity.Id, entity.AssetPairId, entity.Date, entity.Reason, mapTo(entity.Decisions));
        }
        internal List<Decision> mapTo(List<Data.DecisionEntity> decisions)
        {
            List<Decision> mappedDecisions = new List<Decision>();
            decisions.ForEach(element => mappedDecisions.Add(decisionMapper.mapTo(element)));
            return mappedDecisions;
        }
    }
}