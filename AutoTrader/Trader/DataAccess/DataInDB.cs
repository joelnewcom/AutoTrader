using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using AutoTrader.Trader.DataAccess.PoCoToEntityMappers;
using AutoTrader.Trader.Advisor;
using AutoTrader.Models;

namespace AutoTrader.Data
{
    /// This class translate the Pocos into entities, Therefore knows both worlds. This class would need to be rewritten if a new data access is used.
    public class DataInDB : IDataAccess
    {
        private readonly int _timeWindowsInDays = 7;
        private readonly AutoTraderDBContext _context;
        private ILogger<DataInDB> _logger;
        private AssetPairToEntity _assetPairToEntity = new AssetPairToEntity();
        private PriceToEntity _priceToEntity = new PriceToEntity();
        private LogBookMapper _logBookMapper = new LogBookMapper();
        private ExceptionLogMapper _exceptionMapper = new ExceptionLogMapper();
        private DecisionMapper _decisionMapper = new DecisionMapper();

        public DataInDB(ILogger<DataInDB> logger, AutoTraderDBContext autoTraderDBContext)
        {
            _context = autoTraderDBContext;
            _logger = logger;
        }

        public async Task<String> AddAssetPairHistoryEntry(String assetPairId, Price assetPairHistoryEntry)
        {
            var price = await _context.AddAsync(_priceToEntity.mapTo(assetPairHistoryEntry));
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

            return priceEntities.Select(priceEntity => _priceToEntity.mapTo(priceEntity)).ToList();
        }

        public async Task<DateTime> GetDateOfLatestEntry(String assetPairId)
        {
            List<Price> assetPairHistoryEntries = await GetAssetPairHistory(assetPairId);
            if (assetPairHistoryEntries.Count > 1)
                return assetPairHistoryEntries.Last().Date;
            return DateTime.Today.AddDays(-_timeWindowsInDays);
        }

        public async Task<List<AssetPair>> GetAssetPairs()
        {
            List<AssetPairEntity> assetPairToEntities = await _context.assetPairEntities.ToListAsync();
            return assetPairToEntities.Select(assetPairEntity => _assetPairToEntity.mapTo(assetPairEntity)).ToList();
        }

        public async Task<String> AddAssetPair(AssetPair assetPair)
        {
            var entityEntry = await _context.AddAsync(_assetPairToEntity.mapTo(assetPair));
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
            return _assetPairToEntity.mapTo(assetPairEntity);
        }

        public async Task<List<LogBook>> GetLogBook(string assetPairId)
        {
            return await _context.logBooks
            .Where(logBook => logBook.AssetPairId.Equals(assetPairId))
            .Select(entity => _logBookMapper.mapTo(entity))
            .ToListAsync();
        }

        public async Task<string> AddLogBook(LogBook logBook)
        {
            var logBookRecord = await _context.logBooks.AddAsync(_logBookMapper.mapTo(logBook));
            await _context.SaveChangesAsync();
            return logBookRecord.Entity.Id.ToString();
        }

        public async Task<string> UpdateAssetPair(AssetPair assetPair)
        {
            var entityEntry = _context.Update(_assetPairToEntity.mapTo(assetPair));
            await _context.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public Task<int> DeleteAllAssetPair()
        {
            return _context.Database.ExecuteSqlRawAsync("DELETE FROM assetPairEntities");
        }

        public async Task<string> AddExceptionLog(ExceptionLog exceptionLog)
        {
            var logBookRecord = await _context.exceptionLogEnities.AddAsync(_exceptionMapper.create(exceptionLog));
            await _context.SaveChangesAsync();
            return logBookRecord.Entity.Message;
        }

        public async Task<List<ExceptionLog>> GetExceptionLogs()
        {
            return await _context.exceptionLogEnities
            .Select(entity => _exceptionMapper.mapTo(entity))
            .ToListAsync();
        }

        public async Task<List<Decision>> GetDecisions(string logBookId)
        {
            return await _context.DecisionEntities
            .Where(decision => decision.LogBookId.Equals(new Guid(logBookId)))
            .Select(entity => _decisionMapper.mapTo(entity))
            .ToListAsync();
        }
    }
}