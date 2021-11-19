using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Data
{
    public class DataInFile : IDataAccess
    {
        private Dictionary<String, List<AssetPairHistoryEntry>> data = new Dictionary<String, List<AssetPairHistoryEntry>>();
        private Dictionary<String, AssetPair> assetPairs = new Dictionary<String, AssetPair>();

        private readonly ReaderWriterLockSlim _readWriteConfigLock = new ReaderWriterLockSlim();

        private String dataFileLocation = "data.json";

        private String assetPairFileLocation = "assetPair.json";
        private readonly int timeWindowsInDays = 7;

        private ILogger<DataInFile> logger { get; }

        public DataInFile(ILogger<DataInFile> logger)
        {
            loadData();
            this.logger = logger;
        }

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

            PersistData();
            return assetPairId;
        }

        public List<AssetPairHistoryEntry> GetAssetPairHistory(String assetPairId)
        {
            if (assetPairId is null)
            {
                return new List<AssetPairHistoryEntry>();
            }
            List<AssetPairHistoryEntry> assetPairHistoryEntries = data.GetValueOrDefault(assetPairId, new List<AssetPairHistoryEntry>());
            return assetPairHistoryEntries.TakeLast(timeWindowsInDays).ToList();
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
                return assetPairHistoryEntries.Last().Date;
            return DateTime.Today.AddDays(-timeWindowsInDays);
        }

        public List<AssetPair> GetAssetPairs()
        {
            return new List<AssetPair>(assetPairs.Values);
        }

        public void PersistData()
        {
            logger.LogInformation("About to persist the data");
            try
            {
                _readWriteConfigLock.EnterWriteLock();
                string jsonString = JsonSerializer.Serialize(data);
                File.WriteAllText(dataFileLocation, jsonString);
            }
            finally
            {
                _readWriteConfigLock.ExitWriteLock();
            }
        }

        private void loadData()
        {
            if (File.Exists(dataFileLocation))
            {
                String dataString = File.ReadAllText(dataFileLocation);
                data = JsonSerializer.Deserialize<Dictionary<String, List<AssetPairHistoryEntry>>>(dataString);
            }

            if (File.Exists(assetPairFileLocation))
            {
                String dataString = File.ReadAllText(assetPairFileLocation);
                assetPairs = JsonSerializer.Deserialize<Dictionary<String, AssetPair>>(dataString);
            }

        }

        public void AddAssetPair(AssetPair assetPair)
        {
            assetPairs.TryAdd(assetPair.Id, assetPair);
        }
    }
}