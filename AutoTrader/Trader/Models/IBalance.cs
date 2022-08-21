using System;

namespace AutoTrader.Models
{
    public interface IBalance
    {
        String AssetId { get; set; }

        Decimal Available { get; set; }

        Decimal Reserved { get; set; }
    }
}