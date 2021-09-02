using AutoTrader.Data;
using AutoTrader.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTrader.Trader
{
    public class TraderService : IHostedService, IDisposable
    {
        private readonly ILogger<TraderService> _logger;
        private Timer _timer;
        private readonly IRepository repo = new LykkeRepository();

        public TraderService(ILogger<TraderService> logger) =>
            _logger = logger;

        public void Dispose()
        {
            _logger.LogInformation("Got disposed");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(50));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Do work besides host");
            AssetPair assetPair = new AssetPair("ETHCHF", "ETH/CHF", 5);
            Task<IAssetPairHistoryEntry> task = repo.GetHistoryRatePerDay(assetPair, new DateTime(2021, 9, 2));
            IAssetPairHistoryEntry assetPairHistoryEntry = task.Result;
            DataInMemory.Instance.AddAssetPairHistoryEntry(assetPair, assetPairHistoryEntry);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}