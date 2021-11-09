using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoTrader.Repository;
using System.Net.Http;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace AutoTraderTests.Repository
{
    [TestClass()]
    public class LykkeRepositoryTests
    {
        IRepositoryGen<Task<HttpResponseMessage>> repository = new LykkeRepository(new NullLogger<LykkeRepository>(), 
        new TraderConfig { apiKey = "not-valid-jwt-token" });

        [TestMethod()]
        public async Task GetWalletTest()
        {
            HttpResponseMessage message = await repository.GetWallets();
            Assert.IsTrue(System.Net.HttpStatusCode.Unauthorized.Equals(message.StatusCode));
        }
    }
}