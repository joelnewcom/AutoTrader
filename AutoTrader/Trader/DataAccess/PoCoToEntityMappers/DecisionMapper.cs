using System;
using AutoTrader.Data;
using AutoTrader.Trader.Advisor;

namespace AutoTrader.Trader.DataAccess.PoCoToEntityMappers
{
    public class DecisionMapper
    {
        public Decision mapTo(DecisionEntity decisionEntity)
        {
            return new Decision(mapTo(decisionEntity.AdviceType), mapTo(decisionEntity.Advice), decisionEntity.LogBookId);
        }

        public DecisionEntity mapTo(Decision decision)
        {
            return new DecisionEntity(Guid.NewGuid(), mapTo(decision.AdviceType), mapTo(decision.Advice), decision.logBookId);
        }

        internal AdviceType mapTo(AdviceTypeEntity adviceTypeEntity)
        {
            switch (adviceTypeEntity)
            {
                case AdviceTypeEntity.linearSlopeAdvice:
                    return AdviceType.linearSlopeAdvice;
                case AdviceTypeEntity.enoughMoneyAdvice:
                    return AdviceType.enoughMoneyAdvice;
                case AdviceTypeEntity.alreadyOwnerAdvice:
                    return AdviceType.alreadyOwnerAdvice;
                case AdviceTypeEntity.alwaysWinAdvice:
                    return AdviceType.alwaysWinAdvice;
                case AdviceTypeEntity.buySafetyCatch:
                    return AdviceType.buySafetyCatch;
                case AdviceTypeEntity.sellSafetyCatch:
                    return AdviceType.sellSafetyCatch;
                default:
                    throw new ArgumentException("No translation found for " + adviceTypeEntity);
            }
        }

        Advice mapTo(AdviceEntity adviceEntity)
        {
            switch (adviceEntity)
            {
                case AdviceEntity.Buy:
                    return Advice.Buy;
                case AdviceEntity.Sell:
                    return Advice.Sell;
                case AdviceEntity.HoldOn:
                    return Advice.HoldOn;
                default:
                    throw new ArgumentException("No translation found for " + adviceEntity);
            }
        }

        internal AdviceTypeEntity mapTo(AdviceType adviceType)
        {
            switch (adviceType)
            {
                case AdviceType.linearSlopeAdvice:
                    return AdviceTypeEntity.linearSlopeAdvice;
                case AdviceType.alreadyOwnerAdvice:
                    return AdviceTypeEntity.alreadyOwnerAdvice;
                case AdviceType.alwaysWinAdvice:
                    return AdviceTypeEntity.alwaysWinAdvice;
                case AdviceType.buySafetyCatch:
                    return AdviceTypeEntity.buySafetyCatch;
                case AdviceType.sellSafetyCatch:
                    return AdviceTypeEntity.sellSafetyCatch;
                case AdviceType.enoughMoneyAdvice:
                    return AdviceTypeEntity.enoughMoneyAdvice;
                default:
                    throw new ArgumentException("No translation found for " + adviceType);
            }
        }

        AdviceEntity mapTo(Advice advice)
        {
            switch (advice)
            {
                case Advice.Buy:
                    return AdviceEntity.Buy;
                case Advice.Sell:
                    return AdviceEntity.Sell;
                case Advice.HoldOn:
                    return AdviceEntity.HoldOn;
                default:
                    throw new ArgumentException("No translation found for " + advice);
            }
        }

    }
}
