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
    }
}
