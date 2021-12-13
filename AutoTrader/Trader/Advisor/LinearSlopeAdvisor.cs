using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTrader.Advisor
{
    public class LinearSlopeAdvisor : IAdvisor<List<Decimal>>
    {
        int lastDays = 2;
        Decimal slopeThreshold = 0;

        public Advice advice(List<Decimal> dataIn)
        {
            Decimal slopeOverall = LeastSquareMethod.Slope(dataIn);
            Decimal slopeLastDays = LeastSquareMethod.Slope(dataIn.Skip(Math.Max(0, dataIn.Count() - lastDays)));

            if (slopeOverall > slopeThreshold && slopeLastDays < slopeThreshold)
            {
                return Advice.Sell;
            }
            if (slopeOverall < slopeThreshold && slopeLastDays > slopeThreshold)
            {
                return Advice.Buy;
            }

            return Advice.HoldOn;
        }
    }

}