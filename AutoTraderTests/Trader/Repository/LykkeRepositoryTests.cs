using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Config;
using AutoTrader.Repository;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace AutoTraderTests.Trader.Repository
{
    public class LykkeRepositoryTests
    {
        IRepositoryGen<Task<HttpResponseMessage>> repository = new LykkeRepository(new NullLogger<LykkeRepository>(), 
        new TraderConfig { apiKey = "not-valid-jwt-token" });

        [Fact]
        public async Task GetWalletTest()
        {
            HttpResponseMessage message = await repository.GetWallets();
            Assert.IsTrue(System.Net.HttpStatusCode.Unauthorized.Equals(message.StatusCode));
        }
    }
}