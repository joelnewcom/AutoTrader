using AutoTrader.Data;
using AutoTrader.Repository;
using AutoTrader.Trader.PoCos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExceptionLogController : ControllerBase
    {

        private readonly ILogger<ExceptionLogController> _logger;

        private IDataAccessAsync _dataAccess;

        public ExceptionLogController(ILogger<ExceptionLogController> logger, IDataAccessAsync dataAccess, IRepository repo)
        {
            _logger = logger;
            this._dataAccess = dataAccess;
        }

        [HttpGet]
        [Route("api/exceptionlogs")]
        public async Task<List<ExceptionLog>> ExceptionLogs()
        {
            _logger.LogDebug("Called endpoint: Get ExceptionLogs");
            return await _dataAccess.GetExceptionLogs();
        }
    }
}
