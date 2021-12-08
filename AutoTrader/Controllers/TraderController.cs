using AutoTrader.Data;
using AutoTrader.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraderController : ControllerBase
    {

        private readonly ILogger<TraderController> _logger;

        private IDataAccessAsync dataAccess;
        private IRepository repo;

        public TraderController(ILogger<TraderController> logger, IDataAccessAsync dataAccess, IRepository repo)
        {
            _logger = logger;
            this.dataAccess = dataAccess;
            this.repo = repo;
        }


        [HttpGet]
        [Route("api/AssetPairHistoryEntries")]
        public async Task<List<Price>> AssetPairHistoryEntries()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries");
            return await dataAccess.GetAssetPairHistory("ETHCHF");
        }

        [HttpGet]
        [Route("api/AssetPairHistoryEntries/{id}")]
        public async Task<List<Price>> AssetPairHistoryEntries(string id)
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries id: " + id);
            return await dataAccess.GetAssetPairHistory(id);
        }


        [HttpGet]
        [Route("api/AssetPairs")]
        public async Task<List<AssetPair>> AssetPairs()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairs");
            return await dataAccess.GetAssetPairs();
        }

        [HttpGet]
        [Route("api/Trades")]
        public async Task<List<TradeEntry>> Trades()
        {
            _logger.LogDebug("Called endpoint: Get Trades");
            return await repo.GetTrades();
        }
    }
}
