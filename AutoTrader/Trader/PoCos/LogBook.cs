using System;

namespace AutoTrader.Data
{
    public class LogBook
    {
        public Guid Id { get; private set; }
        public String AssetPairId { get; private set; }
        public DateTime Date { get; private set; }
        public String logBookEntry { get; private set; }
        public String reason { get; private set; }

        public LogBook(Guid id, string assetPairId, DateTime date, String logBookEntry, string reason)
        {
            Id = id;
            AssetPairId = assetPairId;
            Date = date;
            this.logBookEntry = logBookEntry;
            this.reason = reason;
        }
    }
}