using System;

namespace AutoTrader.Trader.PoCos
{
    public class Operation
    {
        public Operation(string operationId, string assetId, decimal totalVolume, decimal fee, string type, DateTime timestamp)
        {
            OperationId = operationId;
            AssetId = assetId;
            TotalVolume = totalVolume;
            Fee = fee;
            Type = type;
            Timestamp = timestamp;
        }

        public String OperationId { get; }
        public String AssetId { get; }
        public Decimal TotalVolume { get; }
        public Decimal Fee { get; }
        public String Type { get; }
        public DateTime Timestamp { get; }
    }
}