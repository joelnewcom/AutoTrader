using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class SellIfAlreadyOwned : IAdvisor<String, List<IBalance>>
    {
        public Advice advice(String assetId, List<IBalance> balances)
        {
            foreach (IBalance item in balances)
            {
                if (assetId.Equals(item.AssetId) && item.Available > item.Reserved)
                {
                    return Advice.Sell;
                }
            }

            return Advice.HoldOn;
        }
    }
}