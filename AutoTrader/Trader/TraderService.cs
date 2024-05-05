using AutoTrader.Config;
using AutoTrader.Data;
using AutoTrader.Library;
using AutoTrader.Models;
using AutoTrader.Repository;
using AutoTrader.Trader.Advisor;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly TraderConfig _conf;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly BackgroundTask _backgroundTask;
        private readonly IRepository _repository;
        private readonly IDataAccess _dataAccess;

        public TraderService(ILogger<TraderService> logger, TraderConfig traderConfig, IServiceScopeFactory scopeFactory, BackgroundTask backgroundTask, IRepository repository, 
            IDataAccess dataAccess)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _conf = traderConfig;
            _backgroundTask = backgroundTask;
            _repository = repository;
            _dataAccess = dataAccess;
        }

        public void Dispose()
        {
            _logger.LogInformation("Got disposed");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAssetPairs();
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(_backgroundTask.DoWork, autoEvent, TimeSpan.Zero, TimeSpan.FromHours(8));
        }

        private async Task InitializeAssetPairs()
        {
            _logger.LogInformation("Starting with Prework");
            // using (var scope = _scopeFactory.CreateScope())
            // {
                // IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
                // IDataAccess dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();

                Dictionary<String, AssetPair> assetPairDict = await _repository.GetAssetPairs();
                _logger.LogInformation("Got some assetPairs from repo [assetPairDict]: {keys}", string.Join(", ", assetPairDict.Select(kvp => kvp.Key)));

                List<string> knownAssetPairIds = _conf.knownAssetPairIds;

                _logger.LogInformation("Preconfigured assetPairs [knownAssetPairIds]: {list}", string.Join(", ", knownAssetPairIds.Select(assetPairId => assetPairId)));

                int deletedRows = await _dataAccess.DeleteAllAssetPair();
                _logger.LogInformation("Deleted {0} assetPairs", deletedRows);

                foreach (string item in knownAssetPairIds)
                {
                    AssetPair assetPair = assetPairDict.GetValueOrDefault(item);
                    if (assetPair != null)
                    {
                        AssetPair storedAssetPair = await _dataAccess.GetAssetPair(assetPair.Id);
                        if (storedAssetPair is null)
                        {
                            string v = await _dataAccess.AddAssetPair(assetPair);
                        }
                        else
                        {
                            String task = await _dataAccess.UpdateAssetPair(assetPair);
                        }
                    }
                }

                _logger.LogInformation("Prework is done, following data is prepared [dataAccess]: {dataAccess}", string.Join(", ", (await _dataAccess.GetAssetPairs()).Select(assetPair => assetPair.Id)));
            // }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}