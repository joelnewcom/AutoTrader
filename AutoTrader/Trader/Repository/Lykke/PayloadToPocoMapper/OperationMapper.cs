using System;
using AutoTrader.Models;

namespace AutoTrader.Trader.Repository.Lykke.PocoMapper
{
    public class OperationMapper
    {
        public Operation create(PayloadOperation item)
        {
            if (item is not null)
            {
                return new Operation(item.OperationId, item.AssetId, item.TotalVolume, item.Fee, item.Type, item.Timestamp);
            }

            throw new ArgumentException();
        }

    }
}