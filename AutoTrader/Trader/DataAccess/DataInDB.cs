using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoTrader.Trader.DataAccess.PoCoToEntityMappers;

namespace AutoTrader.Data
{
    public class DataInDB : IDataAccessAsync
    {
        private readonly int timeWindowsInDays = 7;

        private readonly AutoTraderDBContext _context;

        private ILogger<DataInDB> logger { get; }

        private AssetPairToEntity assetPairToEntity = new AssetPairToEntity();
        private PriceToEntity priceToEntity = new PriceToEntity();

        private LogBookMapper logBookMapper = new LogBookMapper();

        public DataInDB(ILogger<DataInDB> logger, AutoTraderDBContext autoTraderDBContext)
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
            List<PriceEntity> priceEntities = await _context.priceEntities
            .Where(p => p.AssetPairId.Equals(assetPairId))
            .OrderBy(p => p.Date)
            .ToListAsync();

            return priceEntities.Select(priceEntity => priceToEntity.create(priceEntity)).ToList();
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

        public async Task<String> AddAssetPair(AssetPair assetPair)
        {
            var entityEntry = await _context.AddAsync(assetPairToEntity.create(assetPair));
            await _context.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public async Task<AssetPair> GetAssetPair(string assetPairId)
        {
            AssetPairEntity assetPairEntity = await _context.assetPairEntities
                .Where(assetPair => assetPair.Id.Equals(assetPairId))
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (assetPairEntity is null)
            {
                return null;
            }
            return assetPairToEntity.create(assetPairEntity);
        }

        public async Task<List<LogBook>> GetLogBook(string assetPairId)
        {
            return await _context.logBooks
            .Where(logBook => logBook.AssetPairId.Equals(assetPairId))
            .Select(entity => logBookMapper.create(entity))
            .ToListAsync();
        }

        public async Task<string> AddLogBook(LogBook logBook)
        {
            var logBookRecord = await _context.logBooks.AddAsync(logBookMapper.create(logBook));
            await _context.SaveChangesAsync();
            return logBookRecord.Entity.Id.ToString();
        }

        public async Task<string> UpdateAssetPair(AssetPair assetPair)
        {
            var entityEntry = _context.Update(assetPairToEntity.create(assetPair));
            await _context.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }
    }
}