using System;
using System.Collections.Generic;

namespace AutoTrader.Data
{
    public interface IDataAccess
    {
        String AddAssetPairHistoryEntry(String assetPairId, AssetPairHistoryEntry assetPairHistoryEntry);

        List<AssetPairHistoryEntry> GetAssetPairHistory(String assetPairId);

        List<float> GetBidHistory(String assetPairId);

        List<float> GetAskHistory(String assetPairId);

        /// Summary:
        /// Returns the newest entry of the specific assetPairId
        DateTime GetDateOfLatestEntry(String assetPairId);

        /// Summary:
        /// Returns all assetPairIds which are available in the store 
        List<AssetPair> GetAssetPairs();
        
        void AddAssetPair(AssetPair assetPair);

        void PersistData();
    }
}
