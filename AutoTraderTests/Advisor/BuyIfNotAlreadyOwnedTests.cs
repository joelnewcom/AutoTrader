using System.Collections.Generic;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoTrader.Advisor
{
    [TestClass()]
    public class BuyIfNotAlreadyOwnedTests
    {

        readonly Mock<IRepository> repoMock = new Mock<IRepository>();

        [TestMethod()]
        public async Task DontBuyIfAlreadyOwned()
        {
            repoMock.Setup(repo => repo.GetWallets()).ReturnsAsync(new List<IWalletEntry>()
            {
                new WalletEntry("LKK", 100, 100)
            });
            
            BuyIfNotAlreadyOwned buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned(repoMock.Object);
            Advice advice = await buyIfNotAlreadyOwned.advice("LKK");

            Assert.AreEqual(Advice.HoldOn, advice);
        }

        [TestMethod()]
        public async Task BuyIfAlreadyOwned()
        {
            repoMock.Setup(repo => repo.GetWallets()).ReturnsAsync(new List<IWalletEntry>()
            {
                new WalletEntry("LKK", 100, 100),
                new WalletEntry("CHF", 100, 100),
                new WalletEntry("BTC", 100, 100),
            });
            
            BuyIfNotAlreadyOwned buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned(repoMock.Object);
            Advice advice = await buyIfNotAlreadyOwned.advice("Eur");

            Assert.AreEqual(Advice.Buy, advice);
        }
    }
}