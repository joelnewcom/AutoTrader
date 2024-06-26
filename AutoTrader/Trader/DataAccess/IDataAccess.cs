﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Models;
using AutoTrader.Trader.Advisor;

namespace AutoTrader.Data
{
    public interface IDataAccess
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

        /// <summary> Updates an assetPair in the store
        Task<String> UpdateAssetPair(AssetPair assertPair);

        /// <summary> Returns the assetPair with the privoded assetPairId from the store </summary>
        Task<AssetPair> GetAssetPair(String assetPairId);

        /// <summary> Returns available logBookentries with the privoded assetPairId </summary>
        Task<List<LogBook>> GetLogBook(String assetPairId);

        /// <summary> To add a new logbook to the store</summary>
        /// <returns> id of the new record </returns>
        Task<String> AddLogBook(LogBook logBook);

        /// <summary> retrieves all Decisions of a logBookEntry</summary>
        /// <returns> List of Decisions </returns>
        Task<List<Decision>> GetDecisions(String logBookId);

        /// <summary> Truncates the whole assetPairs table </summary>
        Task<int> DeleteAllAssetPair();

        /// <summary> Adds new Exception log to store </summary>
        Task<String> AddExceptionLog(ExceptionLog exceptionLog);

        /// <summary>Return all esceptionlogs available in store</summary>
        Task<List<ExceptionLog>> GetExceptionLogs();
    }
}
