namespace AutoTrader.Trader.Advisor
{
    public class DecisionAudit
    {
        public Advice Advice { get; }
        public string Audit { get; }
        public DecisionAudit(Advice advice, string audit){
            Advice = advice;
            Audit = audit;
        }

    }
}