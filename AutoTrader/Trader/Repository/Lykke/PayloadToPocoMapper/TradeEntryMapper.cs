using System;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class TradeEntryMapper
    {
        public TradeEntry build(PayloadTradeHistory item)
        {
            if (item is not null)
            {
                TradeFee tradeFee = new TradeFee(0, "n/a");
                if (item.fee is not null)
                {
                    tradeFee = new TradeFee(item.fee.size, item.fee.assetId);
                }
                return new TradeEntry(item.Id, item.timestamp, item.assetPairId, item.price, item.baseAssetId, item.quoteAssetId, item.role, item.side, item.baseVolume, item.quoteVolume, tradeFee);
            }

            throw new ArgumentException();
        }

    }
}
