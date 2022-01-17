using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrader.Advisor;
using AutoTrader.Config;
using AutoTrader.Data;

namespace AutoTrader.Trader.Advisor
{
    public class Post2DaysDiffSlopeStrategy
    {
        private const Advice buy = Advice.Buy;
        private const Advice sell = Advice.Sell;
        private IAdvisor<List<Decimal>> _linearSlope = new LinearSlopeAdvisor();
        private IAdvisor<String, Price, List<TradeEntry>> _sellAlwaysWin;
        private IAdvisor<String, List<IBalance>> _buyIfNotAlreadyOwned;
        private IAdvisor<String, List<IBalance>> _sellIfAlreadyOwned;
        private IAdvisor<Decimal, List<IBalance>> _buyIfEnoughMoney;
        private Advice _buySafetyCatch;
        private Advice _sellSafetyCatch;
        private int _maxMoneyToSpend { get; }
        public Post2DaysDiffSlopeStrategy(SafetyCatch safetyCatch, int maxMoneyToSpend)
        {
            _sellAlwaysWin = new AlwaysWinSeller();
            _buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned();
            _buyIfEnoughMoney = new BuyIfEnoughCHFAsset();
            _sellIfAlreadyOwned = new SellIfAlreadyOwned();
            _maxMoneyToSpend = maxMoneyToSpend;
            IAdvisor<Advice> safetyCatchAdvisor = new SafetyCatchAdvisor(safetyCatch);
            _buySafetyCatch = safetyCatchAdvisor.advice(buy);
            _sellSafetyCatch = safetyCatchAdvisor.advice(sell);
        }

        public DecisionAudit advice(List<Price> prices, List<IBalance> balances, AssetPair assetPair, List<TradeEntry> trades, Price price)
        {
            IEnumerable<Price> enumerable = prices.Skip(Math.Max(0, prices.Count() - 7));
            List<Decimal> asks = (from Price entry in enumerable select entry.Ask).ToList();

            Advice linearSlopeAdvice = _linearSlope.advice(asks);
            Advice enoughMoneyAdvice = _buyIfEnoughMoney.advice(_maxMoneyToSpend, balances);
            Advice notAlreadyOwnedAdvice = _buyIfNotAlreadyOwned.advice(assetPair.BaseAssetId, balances);
            Advice alreadyOwnerAdvice = _sellIfAlreadyOwned.advice(assetPair.BaseAssetId, balances);
            Advice alwaysWinAdvice = _sellAlwaysWin.advice(assetPair.Id, price, trades);

            String logBookEntry = String.Format(@"Buy Group: [linearSlopeAdvice: {0}, enoughMoneyAdvice: {1}, notAlreadyOwnedAdvice: {2}, buySafetyCatch: {3}], Sell Group: [linearSlopeAdvice: {0}, alreadyOwnerAdvice: {4}, alwaysWinAdvice: {5}, sellSafetyCatch: {6}]", linearSlopeAdvice, enoughMoneyAdvice, notAlreadyOwnedAdvice, _buySafetyCatch, alreadyOwnerAdvice, alwaysWinAdvice, _sellSafetyCatch);

            if (buy.Equals(linearSlopeAdvice) && buy.Equals(enoughMoneyAdvice) && buy.Equals(notAlreadyOwnedAdvice) && buy.Equals(_buySafetyCatch))
            {
                return new DecisionAudit(buy, logBookEntry);
            }

            else if (sell.Equals(linearSlopeAdvice) &&
                sell.Equals(alreadyOwnerAdvice) &&
                sell.Equals(alwaysWinAdvice) &&
                sell.Equals(_sellSafetyCatch))
            {
                return new DecisionAudit(sell, logBookEntry);
            }

            return new DecisionAudit(Advice.HoldOn, logBookEntry);
        }
    }
}