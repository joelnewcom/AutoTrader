using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class BuyIfEnoughCHFAsset : IAdvisor<Decimal, List<IBalance>>
    {

        public Advice advice(Decimal dataIn, List<IBalance> dataIn2)
        {
            foreach (IBalance item in dataIn2)
            {
                if ("CHF".Equals(item.AssetId) && item.Available > dataIn)
                {
                    return Advice.Buy;
                }
            }

            return Advice.HoldOn;
        }
    }
}