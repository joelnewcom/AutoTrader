using AutoTrader.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraderController : ControllerBase
    {

        private readonly ILogger<TraderController> _logger;

        private IDataAccess dataAccess;

        public TraderController(ILogger<TraderController> logger, IDataAccess dataAccess)
        {
            _logger = logger;
            this.dataAccess = dataAccess;
        }

        [HttpGet]
        [Route("api/AssetPairHistoryEntries")]
        public List<AssetPairHistoryEntry> AssetPairHistoryEntries()
        {
            _logger.LogDebug("Called endpoint: Get AssetPairHistoryEntries");
            return dataAccess.GetAssetPairHistory("ETHCHF");
        }

        [HttpGet]
        [Route("api/AssetPairHistoryEntries/{id}")]
        public List<AssetPairHistoryEntry> AssetPairHistoryEntries(string id)
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
    }
}
