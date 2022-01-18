using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class BuyIfEnoughCHFAsset : IAdvisor<Decimal, List<IBalance>>
    {

        public Advice advice(Decimal minAvailable, List<IBalance> balances)
        {
            foreach (IBalance item in balances)
            {
                if ("CHF".Equals(item.AssetId) && (item.Available - item.Reserved) > minAvailable)
                {
                    return Advice.Buy;
                }
            }

            return Advice.HoldOn;
        }
    }
}