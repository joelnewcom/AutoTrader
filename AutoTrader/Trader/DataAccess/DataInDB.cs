using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AutoTrader.Data
{
    public class DataInDB : IDataAccessAsync
    {
        private readonly int timeWindowsInDays = 7;

        private readonly AutoTraderDBContext _context;

        private ILogger<DataInFile> logger { get; }

        private AssetPairToEntity assetPairToEntity = new AssetPairToEntity();
        private PriceToEntity priceToEntity = new PriceToEntity();

        public DataInDB(ILogger<DataInFile> logger, AutoTraderDBContext autoTraderDBContext)
        {
            _context = autoTraderDBContext;
            this.logger = logger;
        }

        public async Task<String> AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry)
        {
            var price = await _context.AddAsync(priceToEntity.create(assetPairHistoryEntry));
            await _context.SaveChangesAsync();
            return price.Entity.AssetPairId;
        }

        public async Task<List<Price>> GetAssetPairHistory(String assetPairId)
        {
            if (assetPairId is null)
            {
                return new List<Price>();
            }
            List<PriceEntity> priceEntities =  await _context.priceEntities
            .Where(p => p.AssetPairId.Equals(assetPairId))
            .OrderBy(p => p.Date)
            .ToListAsync();

            return priceEntities.Select(priceEntity => priceToEntity.create(priceEntity)).ToList();
        }

        public Task<List<float>> GetBidHistory(String assetPairId)
        {
            throw new NotImplementedException();
        }

        public Task<List<float>> GetAskHistory(String assetPairId)
        {
            throw new NotImplementedException();
        }

        public async Task<DateTime> GetDateOfLatestEntry(String assetPairId)
        {
            List<Price> assetPairHistoryEntries = await GetAssetPairHistory(assetPairId);
            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries.Last().Date;
            return DateTime.Today.AddDays(-timeWindowsInDays);
        }

        public async Task<List<AssetPair>> GetAssetPairs()
        {
            List<AssetPairEntity> assetPairToEntities = await _context.assetPairEntities.ToListAsync();
            return assetPairToEntities.Select(assetPairEntity => assetPairToEntity.create(assetPairEntity)).ToList();
        }

        public void PersistData()
        {

        }

        public async Task<String> AddAssetPair(AssetPair assetPair)
        {
            var entityEntry = await _context.AddAsync(assetPairToEntity.create(assetPair));
            await _context.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }
    }
}