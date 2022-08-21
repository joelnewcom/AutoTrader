using System.Collections.Generic;

namespace AutoTrader.Trader.Advisor
{
    public class DecisionAudit
    {
        public Advice Advice { get; }
        public string Audit { get; }

        public List<Decision> Decisions { get; }

        public DecisionAudit(Advice advice, string audit, List<Decision> decisions)
        {
            Advice = advice;
            Audit = audit;
            Decisions = decisions;
        }
    }
}