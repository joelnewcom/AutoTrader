using System;
using System.Collections.Generic;

namespace AutoTrader.Advisor
{
    public interface IAdvisor
    {
        Advice advice(List<float> dataIn);
    }
}
