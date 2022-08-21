using AutoTrader.Models;
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
        private readonly ILogger<WalletController> _logger;

        private IRepository repo;

        public WalletController(ILogger<WalletController> logger, IRepository repo)
        {
            _logger = logger;
            this.repo = repo;
        }

        [HttpGet]
        [Route("api/balance")]
        public async Task<List<IBalance>> Wallets()
        {
            _logger.LogDebug("Called endpoint: Get Wallets");
            return await repo.GetWallets();
        }

        [HttpGet]
        [Route("api/operations")]
        public async Task<List<Operation>> Operations()
        {
            _logger.LogDebug("Called endpoint: Get Operations");
            return await repo.GetOperations();
        }

        [HttpGet]
        [Route("api/prices")]
        public async Task<List<Price>> Prices()
        {
            _logger.LogDebug("Called endpoint: Get Operations");
            return await repo.GetPrices();
        }
    }
}
