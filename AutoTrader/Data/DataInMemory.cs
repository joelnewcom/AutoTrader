using AutoTrader.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public class DataInMemory : IDataAccess
    {
        Dictionary<AssetPair, List<AssetPairHistoryEntry>> data;
        readonly int timeWindowsInDays;

        public DataInMemory(int timeWindowInDays)
        {
            this.timeWindowsInDays = timeWindowInDays;
            data = new Dictionary<AssetPair, List<AssetPairHistoryEntry>>();
        }

        public AssetPair AddAssetPairHistoryEntry(AssetPair assetPair, IAssetPairHistoryEntry assetPairHistoryEntry)
        {
            throw new NotImplementedException();
        }

        public List<AssetPairHistoryEntry> GetAssetPairHistory(AssetPair assetPair)
        {
            throw new NotImplementedException();
        }

        public List<float> GetBidHistory(AssetPair assetPair)
        {
            throw new NotImplementedException();
        }

        public List<float> GetAskHistory(AssetPair assetPair)
        {
            throw new NotImplementedException();
        }

        public DateTime GetYoungestDate(AssetPair assetPair)
        {
            List<AssetPairHistoryEntry> assetPairHistoryEntries = GetAssetPairHistory(assetPair);

            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries[0].Date;
            return DateTime.Now.AddDays(-timeWindowsInDays);
            ;
        }

        public AssetPairHistoryEntry GetYoungestEntry(AssetPair assetPair)
        {
            List<AssetPairHistoryEntry> assetPairHistoryEntries = GetAssetPairHistory(assetPair);

            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries[0];
            return null;
        }
    }
}