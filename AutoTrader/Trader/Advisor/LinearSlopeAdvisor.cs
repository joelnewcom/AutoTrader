using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTrader.Advisor
{
    public class LinearSlopeAdvisor : IAdvisor<List<float>>
    {
        int lastDays = 2;
        float slopeThreshold = 0;

        public Advice advice(List<float> dataIn)
        {
            float slopeOverall = LeastSquareMethod.Slope(dataIn);
            float slopeLastDays = LeastSquareMethod.Slope(dataIn.Skip(Math.Max(0, dataIn.Count() - lastDays)));

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