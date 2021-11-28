﻿using AutoTrader.Advisor;
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
        private const int SECONDS_TO_WAIT = 10;
        private readonly ILogger<TraderService> _logger;
        private Timer _timer;
        private readonly IRepository repo;

        private readonly TraderConfig conf;

        private IDataAccess dataAccess;

        private DataRefresher dataRefresher;

        private IAdvisor<List<float>> linearSlope = new LinearSlopeAdvisor();

        private IAsyncAdvisor<String> alwaysWinSeller;

        private IAsyncAdvisor<String> buyIfNotAlreadyOwned;

        private IAsyncAdvisor<float> buyIfEnoughMoney;

        private int invokeCount;

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
            this.alwaysWinSeller = new AlwaysWinSeller(repo);
            this.buyIfNotAlreadyOwned = new BuyIfNotAlreadyOwned(repo);
            this.buyIfEnoughMoney = new BuyIfEnoughCHFAsset(repo);
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
                    dataAccess.AddAssetPair(assetPair);
                    await dataRefresher.RefreshAssetPairHistory(assetPair.Id);
                }
            }

            _logger.LogInformation("Prework is done, following data is prepared [dataAccess]: {dataAccess}", string.Join(", ", dataAccess.GetAssetPairs().Select(assetPair => assetPair.Id)));
            return;
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
            foreach (AssetPair assetPair in dataAccess.GetAssetPairs())
            {
                List<Price> assetPairHistoryEntries = dataAccess.GetAssetPairHistory(assetPair.Id);
                IEnumerable<Price> enumerable = assetPairHistoryEntries.Skip(Math.Max(0, assetPairHistoryEntries.Count() - 7));

                List<float> asks = (from Price entry in enumerable select entry.Ask).ToList();
                List<float> bids = (from Price entry in enumerable select entry.Bid).ToList();

                if (Advice.Buy.Equals(linearSlope.advice(asks)) &&
                    Advice.Buy.Equals(await buyIfNotAlreadyOwned.advice(assetPair.baseAssetId)) &&
                    Advice.Buy.Equals(await buyIfEnoughMoney.advice(50))
                )
                {
                    _logger.LogInformation("Should buy: " + assetPair.Id);
                }
                else if (Advice.Sell.Equals(linearSlope.advice(bids)) && Advice.Sell.Equals(await alwaysWinSeller.advice(assetPair.Id)))
                {
                    _logger.LogInformation("Should sell: " + assetPair.Id);
                }
                else
                {
                    _logger.LogInformation("Should hold on: " + assetPair.Id);
                }
            }
        }

        private async Task RefreshHistory()
        {
            foreach (AssetPair assetPair in dataAccess.GetAssetPairs())
            {
                await dataRefresher.RefreshAssetPairHistory(assetPair.Id);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            dataAccess.PersistData();
            return Task.CompletedTask;
        }
    }
}