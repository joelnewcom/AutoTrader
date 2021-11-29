using System;

namespace AutoTrader.Repository
{
    public class PayloadWrapper<T>
    {
        public T Payload { get; set; }
        public ErrorPayload Error { get; set; }
    }
}