using System;
using System.Collections.Generic;

namespace AutoTrader.Data
{
    public class DataInMemory : IDataAccess
    {
        private Dictionary<String, List<Price>> data = new Dictionary<String, List<Price>>();
        
        private Dictionary<String, AssetPair> assetPairs = new Dictionary<String, AssetPair>();          
        
        private readonly int timeWindowsInDays = 7;

        public String AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry)
        {
            if (data.ContainsKey(assetPairId))
            {
                List<Price> assetPairHistoryEntries = data.GetValueOrDefault(assetPairId, new List<Price>());
                assetPairHistoryEntries.Add(assetPairHistoryEntry);
            }
            else
            {
                data.Add(assetPairId, new List<Price> { assetPairHistoryEntry });
            }

            return assetPairId;
        }

        public List<Price> GetAssetPairHistory(String assetPairId)
        {
            return data.GetValueOrDefault(assetPairId, new List<Price>());
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
            List<Price> assetPairHistoryEntries = GetAssetPairHistory(assetPairId);
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

        public void PersistData()
        {
            throw new NotImplementedException();
        }
    }
}