using System;

namespace AutoTrader.Data
{
    public class PriceToEntity
    {
        public PriceEntity create(Price item)
        {

            if (item is not null)
            {
                return new PriceEntity(item.Date, item.Ask, item.Bid, item.AssetPairId);
            }

            throw new ArgumentException();
        }

        public Price create (PriceEntity item)
        {

            if (item is not null)
            {
                return new Price(item.Date, item.Ask, item.Bid, item.AssetPairId);
            }

            throw new ArgumentException();
        }

    }
}
