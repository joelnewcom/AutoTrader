using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrader.Advisor;
using AutoTrader.Config;
using AutoTrader.Models;

namespace AutoTrader.Trader.Advisor
{
    public class Post2DaysDiffSlopeStrategy
    {
        private const Advice buy = Advice.Buy;
        private const Advice sell = Advice.Sell;
        private IAdvisor<List<Decimal>> _linearSlope = new LinearSlopeAdvisor();
        private IAdvisor<String, Price, List<TradeEntry>> _sellAlwaysWin;
        private IAdvisor<String, List<IBalance>> _sellIfAlreadyOwned;
        private IAdvisor<Decimal, List<IBalance>> _buyIfEnoughMoney;
        private Advice _buySafetyCatch;
        private Advice _sellSafetyCatch;
        private int _chfToSpend { get; }
        public Post2DaysDiffSlopeStrategy(SafetyCatch safetyCatch, int chfToSpend)
        {
            _sellAlwaysWin = new SellAlwaysWin();
            _buyIfEnoughMoney = new BuyIfEnoughCHFAsset();
            _sellIfAlreadyOwned = new SellIfAlreadyOwned();
            _chfToSpend = chfToSpend;
            IAdvisor<Advice> safetyCatchAdvisor = new SafetyCatchAdvisor(safetyCatch);
            _buySafetyCatch = safetyCatchAdvisor.advice(buy);
            _sellSafetyCatch = safetyCatchAdvisor.advice(sell);
        }

        public DecisionAudit advice(List<Price> prices, List<IBalance> balances, AssetPair assetPair, List<TradeEntry> trades, Price price, Guid logBookId)
        {
            IEnumerable<Price> enumerable = prices.Skip(Math.Max(0, prices.Count() - 7));
            List<Decimal> asks = (from Price entry in enumerable select entry.Ask).ToList();

            Advice linearSlopeAdvice = _linearSlope.advice(asks);
            Advice enoughMoneyAdvice = _buyIfEnoughMoney.advice(_chfToSpend, balances);
            Advice alreadyOwnerAdvice = _sellIfAlreadyOwned.advice(assetPair.BaseAssetId, balances);
            Advice alwaysWinAdvice = _sellAlwaysWin.advice(assetPair.Id, price, trades);

            String logBookEntry = String.Format(@"linearSlopeAdvice: {0}, Buy Group: [enoughMoneyAdvice: {1}, buySafetyCatch: {2}], Sell Group: [alreadyOwnerAdvice: {3}, alwaysWinAdvice: {4}, sellSafetyCatch: {5}]", linearSlopeAdvice, enoughMoneyAdvice, _buySafetyCatch, alreadyOwnerAdvice, alwaysWinAdvice, _sellSafetyCatch);

            List<Decision> decisions = new List<Decision>
            {
                new Decision(AdviceType.linearSlopeAdvice, linearSlopeAdvice, logBookId),
                new Decision(AdviceType.enoughMoneyAdvice, enoughMoneyAdvice, logBookId),
                new Decision(AdviceType.alreadyOwnerAdvice, alreadyOwnerAdvice, logBookId),
                new Decision(AdviceType.alwaysWinAdvice, alwaysWinAdvice, logBookId),
                new Decision(AdviceType.sellSafetyCatch, _sellSafetyCatch, logBookId),
                new Decision(AdviceType.buySafetyCatch, _buySafetyCatch, logBookId)
            };

            if (buy.Equals(linearSlopeAdvice) &&
                buy.Equals(enoughMoneyAdvice) &&
                buy.Equals(_buySafetyCatch))
            {
                return new DecisionAudit(buy, logBookEntry, decisions);
            }

            else if (sell.Equals(linearSlopeAdvice) &&
                sell.Equals(alreadyOwnerAdvice) &&
                sell.Equals(alwaysWinAdvice) &&
                sell.Equals(_sellSafetyCatch))
            {
                return new DecisionAudit(sell, logBookEntry, decisions);
            }

            return new DecisionAudit(Advice.HoldOn, logBookEntry, decisions);
        }
    }
}