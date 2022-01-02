using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public interface IDataAccessAsync
    {
        /// <summary> Stores a new price history entry </summary>
        /// <returns> AssetPairId of new record </returns>
        Task<String> AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry);

        /// <summary> returns the every history price entry the store has available. 
        Task<List<Price>> GetAssetPairHistory(String assetPairId);

        /// <summary> Returns the newest entry of the specific assetPairId </summary>
        Task<DateTime> GetDateOfLatestEntry(String assetPairId);

        /// <summary> Returns all assetPairIds which are available in the store </summary>
        Task<List<AssetPair>> GetAssetPairs();

        /// <summary> Adds an assetpair to the store </summary>
        Task<String> AddAssetPair(AssetPair assetPair);

        /// <summary> Returns the assetPair with the privoded assetPairId from the store </summary>
        Task<AssetPair> GetAssetPair(String assetPairId);

        /// <summary> Returns available logBookentries with the privoded assetPairId </summary>
        Task<List<LogBook>> GetLogBook(String assetPairId);

        /// <summary> To add a new logbook to the store</summary>
        /// <returns> id of the new record </returns>
        Task<String> AddLogBook(LogBook logBook);

    }
}
