using System;

namespace AutoTrader.Trader.Advisor
{
    public class Decision
    {
        public AdviceType AdviceType { get; }
        public Advice Advice { get; }

        public Guid logBookId { get; }
        public Decision(AdviceType auditType, Advice advice, Guid logBookId)
        {
            this.AdviceType = auditType;
            this.Advice = advice;
            this.logBookId = logBookId;
        }

    }
}