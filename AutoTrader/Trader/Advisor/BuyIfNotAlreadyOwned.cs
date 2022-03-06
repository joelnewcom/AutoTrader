using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class BuyIfNotAlreadyOwned : IAdvisor<String, List<IBalance>>
    {
        public Advice advice(String assetId, List<IBalance> balances)
        {
            foreach (IBalance balance in balances)
            {
                if (assetId.Equals(balance.AssetId) && balance.Available > 0)
                {
                    return Advice.HoldOn;
                }
            }
            return Advice.Buy;
        }
    }
}