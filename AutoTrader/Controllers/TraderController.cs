using AutoTrader.Data;
using AutoTrader.Models;
using AutoTrader.Repository;
using AutoTrader.Trader.Advisor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraderController : ControllerBase
    {

        private readonly ILogger<TraderController> _logger;

        private IDataAccess _dataAccess;
        private IRepository _repo;

        public TraderController(ILogger<TraderController> logger, IDataAccess dataAccess, IRepository repo)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _repo = repo;
        }

        [HttpGet]
        [Route("api/AssetPairHistoryEntries/{id}")]
        public async Task<List<Price>> AssetPairHistoryEntries(string id)
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries id: " + id);
            return await _dataAccess.GetAssetPairHistory(id);
        }


        [HttpGet]
        [Route("api/AssetPairs")]
        public async Task<List<AssetPair>> AssetPairs()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairs");
            return await _dataAccess.GetAssetPairs();
        }

        [HttpGet]
        [Route("api/Trades")]
        public async Task<List<TradeEntry>> Trades()
        {
            _logger.LogDebug("Called endpoint: Get Trades");
            return await _repo.GetTrades();
        }

        [HttpGet]
        [Route("api/LogBooks/{assetPair}")]
        public async Task<List<LogBook>> LogBooks(String assetPair)
        {
            _logger.LogDebug("Called endpoint: Get LogBooks");
            return await _dataAccess.GetLogBook(assetPair);
        }

        [HttpGet]
        [Route("api/LogBooks/{logBookId}/Decisions")]
        public async Task<List<Decision>> Decisions(String logBookId)
        {
            _logger.LogDebug("Called endpoint: Get LogBooks");
            return await _dataAccess.GetDecisions(logBookId);
        }

        [HttpGet]
        [Route("api/info")]
        public BackendInfo Info()
        {
            return new BackendInfo(Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.ToString());
        }
    }
}
