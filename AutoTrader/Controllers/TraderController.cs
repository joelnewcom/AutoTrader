using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoTrader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TraderController : ControllerBase
    {
    
        private readonly ILogger<TraderController> _logger;

        public TraderController(ILogger<TraderController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public bool GetSomething()
        {
            return true;
        }
    }
}
