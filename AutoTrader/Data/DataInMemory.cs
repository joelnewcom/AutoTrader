using System;
using System.Collections.Generic;

namespace AutoTrader.Data
{
    public class DataInMemory : IDataAccess
    {
        private Dictionary<String, List<AssetPairHistoryEntry>> data = new Dictionary<String, List<AssetPairHistoryEntry>>();
        
        private Dictionary<String, AssetPair> assetPairs = new Dictionary<String, AssetPair>();          
        
        private readonly int timeWindowsInDays = 7;

        public String AddAssetPairHistoryEntry(String assetPairId, AssetPairHistoryEntry assetPairHistoryEntry)
        {
            if (data.ContainsKey(assetPairId))
            {
                List<AssetPairHistoryEntry> assetPairHistoryEntries = data.GetValueOrDefault(assetPairId, new List<AssetPairHistoryEntry>());
                assetPairHistoryEntries.Add(assetPairHistoryEntry);
            }
            else
            {
                data.Add(assetPairId, new List<AssetPairHistoryEntry> { assetPairHistoryEntry });
            }

            return assetPairId;
        }

        public List<AssetPairHistoryEntry> GetAssetPairHistory(String assetPairId)
        {
            return data.GetValueOrDefault(assetPairId, new List<AssetPairHistoryEntry>());
        }

        public List<float> GetBidHistory(String assetPairId)
        {
            throw new NotImplementedException();
        }

        public List<float> GetAskHistory(String assetPairId)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateOfLatestEntry(String assetPairId)
        {
            List<AssetPairHistoryEntry> assetPairHistoryEntries = GetAssetPairHistory(assetPairId);
            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries[0].Date;
            return DateTime.Today.AddDays(-timeWindowsInDays);
        }

        public List<AssetPair> GetAssetPairs()
        {
            return new List<AssetPair>(assetPairs.Values);
        }

        public void AddAssetPair(AssetPair assetPair)
        {
            throw new NotImplementedException();
        }

        public void PersistData(object stateInfo)
        {
            throw new NotImplementedException();
        }
    }
}