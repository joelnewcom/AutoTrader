using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public interface IDataAccessAsync
    {
        Task<String> AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry);

        Task<List<Price>> GetAssetPairHistory(String assetPairId);

        Task<List<float>> GetBidHistory(String assetPairId);

        Task<List<float>> GetAskHistory(String assetPairId);

        /// Summary:
        /// Returns the newest entry of the specific assetPairId
        Task<DateTime> GetDateOfLatestEntry(String assetPairId);

        /// Summary:
        /// Returns all assetPairIds which are available in the store 
        Task<List<AssetPair>> GetAssetPairs();
        
        /// Summary:
        /// Adds an assetpair to the store
        Task<String> AddAssetPair(AssetPair assetPair);

        /// Summary:
        /// Returns the assetPair from the store
        Task <AssetPair> GetAssetPair(String assetPairId);

    }
}
