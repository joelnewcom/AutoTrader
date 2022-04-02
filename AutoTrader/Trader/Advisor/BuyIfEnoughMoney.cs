using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class BuyIfEnoughCHFAsset : IAdvisor<Decimal, List<IBalance>>
    {

        public Advice advice(Decimal chfToSpend, List<IBalance> balances)
        {
            foreach (IBalance item in balances)
            {
                if ("CHF".Equals(item.AssetId) && (item.Available - item.Reserved) > chfToSpend)
                {
                    return Advice.Buy;
                }
            }

            return Advice.HoldOn;
        }
    }
}