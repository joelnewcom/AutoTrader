using AutoTrader.Data;
using AutoTrader.Library;
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
        private readonly IRepository repo;

        private readonly String[] knownAssets = new String[] { "ETH/CHF" };

        private readonly TraderConfig conf;

        private readonly int secondsToWaitForNextRequest = 10;

        private IDataAccess dataAccess;

        private DataRefresher dataRefresher;

        public TraderService(ILogger<TraderService> logger,
        TraderConfig traderConfig,
        IRepository lykkeRepository,
        IDataAccess dataAccess,
        DataRefresher dataRefresher)
        {
            _logger = logger;
            conf = traderConfig;
            repo = lykkeRepository;
            this.dataAccess = dataAccess;
            this.dataRefresher = dataRefresher;
        }

        public void Dispose()
        {
            _logger.LogInformation("Got disposed");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            doPrepWorkAsync();
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(50));
            return Task.CompletedTask;
        }

        private async void doPrepWorkAsync()
        {
            Dictionary<String, AssetPair> assetPairDict = await repo.GetAssetPairsDictionary();

            List<string> knownAssetPairIds = conf.knownAssetPairIds;
            foreach (string item in knownAssetPairIds)
            {
                AssetPair assetPair = assetPairDict.GetValueOrDefault(item);
                if (assetPair != null)
                {
                    dataRefresher.RefreshAssetPairHistory(assetPair);
                    await Task.Delay(secondsToWaitForNextRequest * 1000);
                }
            }
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Lykke trader does work");

            foreach (AssetPair assetPair in dataAccess.GetAssetPairs())
            {
                dataRefresher.RefreshAssetPairHistory(assetPair);
                await Task.Delay(secondsToWaitForNextRequest * 1000);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}