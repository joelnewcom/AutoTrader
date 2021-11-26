using System;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class PriceMapper
    {
        public Price build(PayloadPrice price)
        {
            if (price is not null)
            {
                return new Price(price.TimeStamp, price.ask, price.bid);
            }

            throw new ArgumentException();
        }

    }
}
