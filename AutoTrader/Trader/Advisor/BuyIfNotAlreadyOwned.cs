using System;
using System.Collections.Generic;
using AutoTrader.Data;

namespace AutoTrader.Advisor
{
    public class BuyIfNotAlreadyOwned : IAdvisor<String, List<IBalance>>
    {
        public Advice advice(String dataIn, List<IBalance> dataIn2)
        {
            foreach (IBalance item in dataIn2)
            {
                if (dataIn.Equals(item.AssetId))
                {
                    return Advice.HoldOn;
                }
            }

            return Advice.Buy;
        }
    }
}