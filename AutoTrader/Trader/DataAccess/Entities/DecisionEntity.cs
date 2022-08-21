using System;

namespace AutoTrader.Data
{
    /// Private setters are only for EntityFramework Core. Without any Setter, the EntityFramework would not recognise it as a field (Even when they are part of the constructor)
    public class DecisionEntity
    {
        public Guid Id { get; private set;}
        public AdviceTypeEntity AdviceType { get; private set;  }
        public AdviceEntity Advice { get; private set;  }

        // DecisionEntity.LogBookId is the foreignKey
        public Guid LogBookId { get; private set; }

        // DecisionEntity.LogBookEntity is a reference navigation property. EF core doesn't support them as constructor params
        public LogBookEntity LogBookEntity { get; set; }

        public DecisionEntity(Guid id, AdviceTypeEntity adviceType, AdviceEntity advice, Guid logBookId)
        {
            this.Id = id;
            this.AdviceType = adviceType;
            this.Advice = advice;
            this.LogBookId = logBookId;
        }
    }
}