using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Data
{
    public class DataInMemory : IDataAccessAsync
    {
        private Dictionary<String, List<Price>> data = new Dictionary<String, List<Price>>();

        private Dictionary<String, AssetPair> assetPairs = new Dictionary<String, AssetPair>();

        private readonly int timeWindowsInDays = 7;

        public ILogger<DataInMemory> logger { get; }

        public DataInMemory(ILogger<DataInMemory> logger)
        {
            this.logger = logger;
        }


        public Task<String> AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry)
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

            return Task.FromResult(assetPairId);
        }

        public Task<List<Price>> GetAssetPairHistory(String assetPairId)
        {
            return Task.FromResult(data.GetValueOrDefault(assetPairId, new List<Price>()));
        }

        public async Task<DateTime> GetDateOfLatestEntry(String assetPairId)
        {
            List<Price> assetPairHistoryEntries = await GetAssetPairHistory(assetPairId);
            assetPairHistoryEntries.Sort(delegate (Price x, Price y)
            {
                if (x.Date == null && y.Date == null) return 0;
                else if (x.Date == null) return -1;
                else if (y.Date == null) return 1;
                else return y.Date.CompareTo(x.Date);
            });

            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries[0].Date;
            return DateTime.Today.AddDays(-timeWindowsInDays);
        }

        public Task<List<AssetPair>> GetAssetPairs()
        {
            return Task.FromResult(new List<AssetPair>(assetPairs.Values));
        }

        Task<string> IDataAccessAsync.AddAssetPair(AssetPair assetPair)
        {
            throw new NotImplementedException();
        }

        public Task<AssetPair> GetAssetPair(string assetPairId)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogBook>> GetLogBook(string assetPairId)
        {
            throw new NotImplementedException();
        }

        public Task<string> AddLogBook(LogBook logBook)
        {
            throw new NotImplementedException();
        }
    }
}