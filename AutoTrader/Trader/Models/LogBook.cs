using System;
using System.Collections.Generic;
using AutoTrader.Trader.Advisor;

namespace AutoTrader.Models
{
    public class LogBook
    {
        public Guid Id { get; private set; }
        public String AssetPairId { get; private set; }
        public DateTime Date { get; private set; }
        public String reason { get; private set; }
        public List<Decision> decisions { get; private set; }

        public LogBook(Guid id, string assetPairId, DateTime date, string reason, List<Decision> decisions)
        {
            Id = id;
            AssetPairId = assetPairId;
            Date = date;
            this.reason = reason;
            this.decisions = decisions;
        }
    }
}