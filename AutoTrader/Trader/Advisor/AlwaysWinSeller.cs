using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Advisor
{
    public class AlwaysWinSeller : IAsyncAdvisor<String>
    {

        IRepository repo;
        public AlwaysWinSeller(IRepository lykkeRepository)
        {
            this.repo = lykkeRepository;
        }

        public async Task<Advice> advice(string assetPairId)
        {
            IPrice price = await repo.GetPrice(assetPairId);
            if (price is Price && await isHigherThanOurPrice((Price)price, assetPairId))
            {
                return Advice.Sell;
            }
            return Advice.HoldOn;
        }

        private async Task<bool> isHigherThanOurPrice(Price price, String assetPairId)
        {
            List<TradeEntry> trades = await repo.GetTrades();
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