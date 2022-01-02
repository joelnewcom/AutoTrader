using AutoTrader.Advisor;
using AutoTrader.Config;
using AutoTrader.Data;
using AutoTrader.Library;
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
        private const int CHF_TO_SPEND_AT_ONCE = 50;
        private const Advice buy = Advice.Buy;
        private const Advice sell = Advice.Sell;
        private readonly ILogger<TraderService> _logger;
        private Timer _timer;

        private readonly TraderConfig conf;

        private int invokeCount;

        private readonly IServiceScopeFactory scopeFactory;

        private Post2DaysDiffSlopeStrategy post2DaysDiffSlopeStrategy;

        public TraderService(ILogger<TraderService> logger, TraderConfig traderConfig, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
            conf = traderConfig;
            this.post2DaysDiffSlopeStrategy = new Post2DaysDiffSlopeStrategy(conf.safetyCatch, CHF_TO_SPEND_AT_ONCE);
        }

        public void Dispose()
        {
            _logger.LogInformation("Got disposed");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await doPrepWorkAsync();
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(DoWork, autoEvent, TimeSpan.Zero, TimeSpan.FromHours(8));
            return;
        }

        private async Task doPrepWorkAsync()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
                IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();
                DataRefresher dataRefresher = scope.ServiceProvider.GetRequiredService<DataRefresher>();

                _logger.LogInformation("Starting with Prework");
                Dictionary<String, AssetPair> assetPairDict = await repo.GetAssetPairsDictionary();
                _logger.LogInformation("Got some assetPairs from repo [assetPairDict]: {keys}", string.Join(", ", assetPairDict.Select(kvp => kvp.Key)));

                List<string> knownAssetPairIds = conf.knownAssetPairIds;

                _logger.LogInformation("Preconfigured assetPairs [knownAssetPairIds]: {list}", string.Join(", ", knownAssetPairIds.Select(assetPairId => assetPairId)));
                foreach (string item in knownAssetPairIds)
                {
                    AssetPair assetPair = assetPairDict.GetValueOrDefault(item);
                    if (assetPair != null)
                    {
                        AssetPair storedAssetPair = await dataAccess.GetAssetPair(assetPair.Id);
                        if (storedAssetPair is null)
                        {
                            await dataAccess.AddAssetPair(assetPair);
                        }
                        await dataRefresher.RefreshAssetPairHistory(assetPair.Id);
                    }
                }

                _logger.LogInformation("Prework is done, following data is prepared [dataAccess]: {dataAccess}", string.Join(", ", (await dataAccess.GetAssetPairs()).Select(assetPair => assetPair.Id)));
                return;
            }
        }

        private async void DoWork(object stateInfo)
        {
            _logger.LogInformation("Lykke trader started to do work");
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Console.WriteLine("Time: {0}, InvokeCount:  {1,2}.", DateTime.Now.ToString("h:mm:ss.fff"), (++invokeCount).ToString());
            await RefreshHistory();
            trade();
        }

        private async void trade()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
                IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();
                List<TradeEntry> trades = await repo.GetTrades();
                List<IBalance> balances = await repo.GetWallets();

                foreach (AssetPair assetPair in await dataAccess.GetAssetPairs())
                {
                    List<Price> assetPairHistoryEntries = await dataAccess.GetAssetPairHistory(assetPair.Id);
                    IPrice iPrice = await repo.GetPrice(assetPair.Id);

                    if (iPrice is not Price)
                    {
                        continue;
                    }

                    Price price = (Price)iPrice;

                    DecisionAudit decisionAudit = post2DaysDiffSlopeStrategy.advice(assetPairHistoryEntries, balances, assetPair, trades, price);
                    String reason = "unknown";

                    if (buy.Equals(decisionAudit.Advice))
                    {
                        Decimal volume = CHF_TO_SPEND_AT_ONCE / price.Ask;
                        _logger.LogInformation("Should buy: {0}, volume: {1}", assetPair.Id, volume);
                        string orderId = await repo.LimitOrderBuy(assetPair.Id, price.Ask, Decimal.Round(volume, assetPair.Accuracy));
                        reason = "buy orderId: " + orderId;
                        balances = await repo.GetWallets();
                    }

                    else if (sell.Equals(decisionAudit))
                    {
                        IBalance balance = balances.Where(b => assetPair.BaseAssetId.Equals(b.AssetId)).First();
                        _logger.LogInformation("Should sell: {0}, volume: {1}", assetPair.Id, balance.Available);
                        string orderId = await repo.LimitOrderSell(assetPair.Id, price.Ask, Decimal.Round(balance.Available, assetPair.Accuracy));
                        reason = "sell orderId: " + orderId;
                        balances = await repo.GetWallets();
                    }
                    else
                    {
                        _logger.LogInformation("Should hold on: " + assetPair.Id);
                        reason = "Hold on";
                    }

                    await dataAccess.AddLogBook(new LogBook(Guid.NewGuid(), assetPair.Id, DateTime.Now, decisionAudit.Audit, reason));

                }
            }
        }

        private async Task RefreshHistory()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                DataRefresher dataRefresher = scope.ServiceProvider.GetRequiredService<DataRefresher>();
                IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();

                foreach (AssetPair assetPair in await dataAccess.GetAssetPairs())
                {
                    await dataRefresher.RefreshAssetPairHistory(assetPair.Id);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //await dataAccess.PersistData();
            return Task.CompletedTask;
        }
    }
}