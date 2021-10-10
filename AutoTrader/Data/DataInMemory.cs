using System;
using System.Collections.Generic;

namespace AutoTrader.Data
{
    public class DataInMemory : IDataAccess
    {
        private Dictionary<AssetPair, List<IAssetPairHistoryEntry>> data = new Dictionary<AssetPair, List<IAssetPairHistoryEntry>>();
        private readonly int timeWindowsInDays = 7;

        // private DataInMemory()
        // {
        // }

        // private static readonly Lazy<DataInMemory> lazy = new Lazy<DataInMemory>(() => new DataInMemory());

        // public static DataInMemory Instance
        // {
        //     get
        //     {
        //         return lazy.Value;
        //     }
        // }

        public AssetPair AddAssetPairHistoryEntry(AssetPair assetPair, IAssetPairHistoryEntry assetPairHistoryEntry)
        {
            if (data.ContainsKey(assetPair))
            {
                List<IAssetPairHistoryEntry> assetPairHistoryEntries = data.GetValueOrDefault(assetPair, new List<IAssetPairHistoryEntry>());
                assetPairHistoryEntries.Add(assetPairHistoryEntry);
            }
            else
            {
                data.Add(assetPair, new List<IAssetPairHistoryEntry> { assetPairHistoryEntry });
            }

            return assetPair;
        }

        public List<IAssetPairHistoryEntry> GetAssetPairHistory(AssetPair assetPair)
        {
            return data.GetValueOrDefault(assetPair, new List<IAssetPairHistoryEntry>());
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
            List<IAssetPairHistoryEntry> assetPairHistoryEntries = GetAssetPairHistory(assetPair);
            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries[0].Date;
            return DateTime.Today.AddDays(-timeWindowsInDays);
        }

        public List<AssetPair> GetAssetPairs()
        {
            return new List<AssetPair>(this.data.Keys);
        }
    }
}