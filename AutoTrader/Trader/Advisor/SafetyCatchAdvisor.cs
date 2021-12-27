using AutoTrader.Config;

namespace AutoTrader.Advisor
{
    public class SafetyCatchAdvisor : IAdvisor<Advice>
    {

        SafetyCatch safetyFlag = new SafetyCatch { buySafetyCatch = false, sellSafetyCatch = false };
        public SafetyCatchAdvisor(SafetyCatch safetyFlag)
        {
            this.safetyFlag = safetyFlag;
        }

        public Advice advice(Advice advice)
        {
            switch (advice)
            {
                case Advice.Buy:
                    return safetyFlag.buySafetyCatch ? Advice.HoldOn : Advice.Buy;
                case Advice.Sell:
                    return safetyFlag.sellSafetyCatch ? Advice.HoldOn : Advice.Sell;
                default:
                    return Advice.HoldOn;
            }
        }
    }
}