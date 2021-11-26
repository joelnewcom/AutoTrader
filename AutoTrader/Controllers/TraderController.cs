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

        private IDataAccess dataAccess;
        private IRepository repo;

        public TraderController(ILogger<TraderController> logger, IDataAccess dataAccess, IRepository repo)
        {
            _logger = logger;
            this.dataAccess = dataAccess;
            this.repo = repo;
        }


        [HttpGet]
        [Route("api/AssetPairHistoryEntries")]
        public List<Price> AssetPairHistoryEntries()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries");
            return dataAccess.GetAssetPairHistory("ETHCHF");
        }

        [HttpGet]
        [Route("api/AssetPairHistoryEntries/{id}")]
        public List<Price> AssetPairHistoryEntries(string id)
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries id: " + id);
            return dataAccess.GetAssetPairHistory(id);
        }


        [HttpGet]
        [Route("api/AssetPairs")]
        public List<AssetPair> AssetPairs()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairs");
            List<AssetPair> assetPair = dataAccess.GetAssetPairs();
            return assetPair;
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
