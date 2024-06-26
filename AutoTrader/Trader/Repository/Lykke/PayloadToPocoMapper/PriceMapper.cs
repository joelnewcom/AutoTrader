using System;
using AutoTrader.Data;
using AutoTrader.Models;
using AutoTrader.Repository;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class PriceMapper
    {
        public Price build(PayloadPrice price)
        {
            if (price is not null)
            {
                return new Price(price.timestamp, price.ask, price.bid, price.assetPairId);
            }

            throw new ArgumentException();
        }

    }
}
