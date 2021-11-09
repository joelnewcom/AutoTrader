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
    public class WalletController : ControllerBase
    {

        private readonly ILogger<TraderController> _logger;

        private IDataAccess dataAccess;

        private IRepository repo;

        public WalletController(ILogger<TraderController> logger, IDataAccess dataAccess, IRepository repo)
        {
            _logger = logger;
            this.dataAccess = dataAccess;
            this.repo = repo;
        }

        [HttpGet]
        [Route("api/balance")]
        public async Task<List<IWalletEntry>> Wallets()
        {
            _logger.LogDebug("Called endpoint: Get Wallets");
            return await repo.GetWallets();
        }
    }
}
