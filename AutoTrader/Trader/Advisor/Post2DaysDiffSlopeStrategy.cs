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
        private IAdvisor<List<Decimal>> linearSlope = new LinearSlopeAdvisor();

        private IAdvisor<String, Price, List<TradeEntry>> alwaysWinSeller;

        private IAdvisor<String, List<IBalance>> buyIfNotAlreadyOwned;

        private IAdvisor<String, List<IBalance>> sellIfAlreadyOwned;

        private IAdvisor<Decimal, List<IBalance>> buyIfEnoughMoney;

        private Advice buySafetyCatch;
        private Advice sellSafetyCatch;
        private int maxMoneyToSpend { get; }
        public Post2DaysDiffSlopeStrategy(SafetyCatch safetyCatch, int maxMoneyToSpend)
        {
            this.alwaysWinSeller = new AlwaysWinSeller();
            this.buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned();
            this.buyIfEnoughMoney = new BuyIfEnoughCHFAsset();
            this.sellIfAlreadyOwned = new SellIfAlreadyOwned();
            this.maxMoneyToSpend = maxMoneyToSpend;
            IAdvisor<Advice> safetyCatchAdvisor = new SafetyCatchAdvisor(safetyCatch);
            buySafetyCatch = safetyCatchAdvisor.advice(buy);
            sellSafetyCatch = safetyCatchAdvisor.advice(sell);
        }

        public DecisionAudit advice(List<Price> prices, List<IBalance> balances, AssetPair assetPair, List<TradeEntry> trades, Price price)
        {
            IEnumerable<Price> enumerable = prices.Skip(Math.Max(0, prices.Count() - 7));
            List<Decimal> asks = (from Price entry in enumerable select entry.Ask).ToList();

            Advice linearSlopeAdvice = linearSlope.advice(asks);
            Advice enoughMoneyAdvice = buyIfEnoughMoney.advice(maxMoneyToSpend, balances);
            Advice notAlreadyOwnedAdvice = buyIfNotAlreadyOwned.advice(assetPair.BaseAssetId, balances);
            Advice alreadyOwnerAdvice = sellIfAlreadyOwned.advice(assetPair.BaseAssetId, balances);
            Advice alwaysWinAdvice = alwaysWinSeller.advice(assetPair.Id, price, trades);

            String logBookEntry = String.Format(@"Buy Group: [   
                                                    linearSlopeAdvice: {0}, 
                                                    enoughMoneyAdvice: {1}, 
                                                    notAlreadyOwnedAdvice: {2}, 
                                                    buySafetyCatch: {3}], 
                                                Sell Group: [
                                                    linearSlopeAdvice: {0}, 
                                                    alreadyOwnerAdvice: {4}, 
                                                    alwaysWinAdvice: {5}, 
                                                    sellSafetyCatch: {6}
                                                ]", linearSlopeAdvice, enoughMoneyAdvice, notAlreadyOwnedAdvice, buySafetyCatch, alreadyOwnerAdvice, alwaysWinAdvice, sellSafetyCatch);

            

            if (buy.Equals(linearSlopeAdvice) && buy.Equals(enoughMoneyAdvice) && buy.Equals(notAlreadyOwnedAdvice) && buy.Equals(buySafetyCatch))
            {
                return new DecisionAudit(buy, logBookEntry);
            }

            else if (sell.Equals(linearSlopeAdvice) &&
                sell.Equals(alreadyOwnerAdvice) &&
                sell.Equals(alwaysWinAdvice) &&
                sell.Equals(sellSafetyCatch))
            {
                return new DecisionAudit(sell, logBookEntry);
            }

            return new DecisionAudit(Advice.HoldOn, logBookEntry);
        }

    }
}