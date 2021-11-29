using System;

namespace AutoTrader.Data
{
    public interface IBalance
    {
        String AssetId { get; set; }

        float Available { get; set; }

        float Reserved { get; set; }
    }
}