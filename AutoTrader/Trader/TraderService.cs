using AutoTrader.Advisor;
using AutoTrader.Data;
using AutoTrader.Library;
using AutoTrader.Repository;
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
        private const int SECONDS_TO_WAIT = 10;
        private const int CHF_TO_SPEND_AT_ONCE = 50;
        private readonly ILogger<TraderService> _logger;
        private Timer _timer;
        // private readonly IRepository repo;

        private readonly TraderConfig conf;

        // private IDataAccessAsync dataAccess;

        // private DataRefresher dataRefresher;

        private IAdvisor<List<float>> linearSlope = new LinearSlopeAdvisor();

        private IAdvisor<String, Price, List<TradeEntry>> alwaysWinSeller;

        private IAdvisor<String, List<IBalance>> buyIfNotAlreadyOwned;

        private IAdvisor<float, List<IBalance>> buyIfEnoughMoney;

        private int invokeCount;

        private readonly IServiceScopeFactory scopeFactory;

        public TraderService(ILogger<TraderService> logger,
        TraderConfig traderConfig,
        // IRepository lykkeRepository,
        // IDataAccessAsync dataAccess,
        // DataRefresher dataRefresher,
        IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            this.scopeFactory = scopeFactory;
            conf = traderConfig;

            // this.repo = lykkeRepository;
            // this.dataAccess = dataAccess;
            // this.dataRefresher = dataRefresher;

            this.alwaysWinSeller = new AlwaysWinSeller();
            this.buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned();
            this.buyIfEnoughMoney = new BuyIfEnoughCHFAsset();

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
                        await dataAccess.AddAssetPair(assetPair);
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

                foreach (AssetPair assetPair in await dataAccess.GetAssetPairs())
                {
                    List<Price> assetPairHistoryEntries = await dataAccess.GetAssetPairHistory(assetPair.Id);
                    IEnumerable<Price> enumerable = assetPairHistoryEntries.Skip(Math.Max(0, assetPairHistoryEntries.Count() - 7));

                    List<float> asks = (from Price entry in enumerable select entry.Ask).ToList();
                    List<float> bids = (from Price entry in enumerable select entry.Bid).ToList();
                    IPrice iPrice = await repo.GetPrice(assetPair.Id);
                    List<TradeEntry> trades = await repo.GetTrades();
                    List<IBalance> balances = await repo.GetWallets();

                    if (iPrice is not Price)
                    {
                        continue;
                    }

                    Price price = (Price)iPrice;

                    if (Advice.Buy.Equals(linearSlope.advice(asks)))
                    {
                        if (Advice.Buy.Equals(buyIfEnoughMoney.advice(CHF_TO_SPEND_AT_ONCE, balances)) && Advice.Buy.Equals(buyIfNotAlreadyOwned.advice(assetPair.BaseAssetId, balances)))
                        {
                            _logger.LogInformation("Should buy: " + assetPair.Id);
                            float volume = CHF_TO_SPEND_AT_ONCE / price.Ask;
                            //Task<string> orderId = repo.LimitOrderBuy(assetPair.Id, price.Ask, volume);
                        }
                    }

                    else if (Advice.Sell.Equals(linearSlope.advice(bids)) && Advice.Sell.Equals(alwaysWinSeller.advice(assetPair.Id, price, trades)))
                    {
                        _logger.LogInformation("Should sell: " + assetPair.Id);
                    }
                    else
                    {
                        _logger.LogInformation("Should hold on: " + assetPair.Id);
                    }
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