using System;

namespace AutoTrader.Repository
{
    public class PayloadWrapper<T>
    {
        public T Payload { get; set; }
        public ErrorPayload Error { get; set; }
        public float Ask { get; set; }
        public float TradingVolume { get; set; }
        public float TradingOppositeVolume { get; set; }
    }

}