using System;
using AutoTrader.Data;
using AutoTrader.Repository;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class AssetPairHistoryEntryMapper
    {
        public Price create(PayloadResponseGetHistoryRate item, DateTime dateTime)
        {
            if (item is not null)
            {
                return new Price(dateTime, item.Ask, item.Bid);
            }

            throw new ArgumentException();
        }

    }
}
