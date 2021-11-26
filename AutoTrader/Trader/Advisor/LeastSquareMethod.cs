using System.Collections.Generic;
using System.Linq;

namespace AutoTrader.Advisor
{
    public class LeastSquareMethod
    {
        public static float Slope(IEnumerable<float> dataIn)
        {

            // Equation: m =  (n*sum(xy) - sum(x)*sum(y)) / (n*sum(x^2) - sum(x)^2)
            int n = dataIn.Count();
            int x = 1;
            int sumX = 0;
            float sumY = 0;
            float sumXY = 0;
            float sumX2 = 0;
            foreach (float y in dataIn)
            {
                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
                x++;
            }

            return (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        }
    }
}