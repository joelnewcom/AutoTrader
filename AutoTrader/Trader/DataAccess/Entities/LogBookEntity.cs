using System;
using System.Collections.Generic;

namespace AutoTrader.Data
{
    /// Private setters are only for EntityFramework Core. Without any Setter, the EntityFramework would not recognise it as a field (Even when they are part of the constructor)
    public class LogBookEntity
    {
        public Guid Id { get; private set; }
        public String AssetPairId { get; private set; }
        public DateTime Date { get; private set; }
        public String Reason { get; private set; }

        // LogBookEntitry.Decisions is a reference navigation property. EF core doesn't support them as constructor params
        public List<DecisionEntity> Decisions { get; set; } = new List<DecisionEntity>();

        public LogBookEntity(Guid id, string assetPairId, DateTime date, string reason)
        {
            this.Id = id;
            this.AssetPairId = assetPairId;
            this.Date = date;
            this.Reason = reason;
        }
    }
}