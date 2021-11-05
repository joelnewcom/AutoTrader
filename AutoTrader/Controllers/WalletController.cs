using AutoTrader.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalletController : ControllerBase
    {

        private readonly ILogger<TraderController> _logger;

        private IDataAccess dataAccess;

        public WalletController(ILogger<TraderController> logger, IDataAccess dataAccess)
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
    }
}
