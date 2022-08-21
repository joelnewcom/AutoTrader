using System;
using System.Collections.Generic;
using System.Linq;
using AutoTrader.Data;
using AutoTrader.Models;

namespace AutoTrader.Advisor
{
    public class SellAlwaysWin : IAdvisor<String, Price, List<TradeEntry>>
    {
        public Advice advice(string assetPairId, Price price, List<TradeEntry> trades)
        {
            if (price is not null && isHigherThanOurPrice(price, assetPairId, trades))
            {
                return Advice.Sell;
            }
            return Advice.HoldOn;
        }

        private bool isHigherThanOurPrice(Price price, String assetPairId, List<TradeEntry> trades)
        {
            IEnumerable<TradeEntry> sortedTrades = trades.Where(trade => trade.assetPairId.Equals(assetPairId) && "buy".Equals(trade.side)).OrderBy(trade => trade.timestamp);
            if (sortedTrades.Count() > 0)
            {
                TradeEntry tradeEntry = sortedTrades.Last();
                return price.Bid > tradeEntry.price;
            }
            return true;
        }

    }
}