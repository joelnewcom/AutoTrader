using AutoTrader.Config;
using AutoTrader.Data;
using AutoTrader.Library;
using AutoTrader.Repository;
using AutoTrader.Trader.Advisor;
using AutoTrader.Trader.PoCos;
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
        private const Advice BUY = Advice.Buy;
        private const Advice SELL = Advice.Sell;
        private readonly ILogger<TraderService> _logger;
        private Timer _timer;
        private readonly TraderConfig _conf;
        private int _invokeCount;
        private readonly IServiceScopeFactory _scopeFactory;
        private Post2DaysDiffSlopeStrategy _post2DaysDiffSlopeStrategy;

        public TraderService(ILogger<TraderService> logger, TraderConfig traderConfig, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _conf = traderConfig;
            _post2DaysDiffSlopeStrategy = new Post2DaysDiffSlopeStrategy(_conf.safetyCatch, CHF_TO_SPEND_AT_ONCE);
        }

        public void Dispose()
        {
            _logger.LogInformation("Got disposed");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitializeAssetPairs();
            var autoEvent = new AutoResetEvent(false);
            _timer = new Timer(DoWork, autoEvent, TimeSpan.Zero, TimeSpan.FromHours(8));
            return;
        }

        private async Task InitializeAssetPairs()
        {
            _logger.LogInformation("Starting with Prework");
            using (var scope = _scopeFactory.CreateScope())
            {
                IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
                IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();
                DataRefresher dataRefresher = scope.ServiceProvider.GetRequiredService<DataRefresher>();

                Dictionary<String, AssetPair> assetPairDict = await repo.GetAssetPairs();
                _logger.LogInformation("Got some assetPairs from repo [assetPairDict]: {keys}", string.Join(", ", assetPairDict.Select(kvp => kvp.Key)));

                List<string> knownAssetPairIds = _conf.knownAssetPairIds;

                _logger.LogInformation("Preconfigured assetPairs [knownAssetPairIds]: {list}", string.Join(", ", knownAssetPairIds.Select(assetPairId => assetPairId)));

                int deletedRows = await dataAccess.DeleteAllAssetPair();
                _logger.LogInformation("Deleted {0} assetPairs", deletedRows);

                foreach (string item in knownAssetPairIds)
                {
                    AssetPair assetPair = assetPairDict.GetValueOrDefault(item);
                    if (assetPair != null)
                    {
                        AssetPair storedAssetPair = await dataAccess.GetAssetPair(assetPair.Id);
                        if (storedAssetPair is null)
                        {
                            string v = await dataAccess.AddAssetPair(assetPair);
                        }
                        else
                        {
                            String task = await dataAccess.UpdateAssetPair(assetPair);
                        }
                    }
                }

                _logger.LogInformation("Prework is done, following data is prepared [dataAccess]: {dataAccess}", string.Join(", ", (await dataAccess.GetAssetPairs()).Select(assetPair => assetPair.Id)));
                return;
            }
        }

        private async void DoWork(object stateInfo)
        {
            try
            {
                _logger.LogInformation("Lykke trader started to do work");
                AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
                Console.WriteLine("Time: {0}, InvokeCount:  {1,2}.", DateTime.Now.ToString("h:mm:ss.fff"), (++_invokeCount).ToString());
                await RefreshHistory();
                Trade();
            }
            catch (Exception e)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();
                    String exceptionLog = await dataAccess.AddExceptionLog(new ExceptionLog(Guid.NewGuid(), e.Message, DateTime.Now));
                    _logger.LogError("Catched unexpected exception {0}", exceptionLog);
                }
            }
        }

        private async void Trade()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
                IDataAccessAsync dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccessAsync>();
                List<TradeEntry> trades = await repo.GetTrades();
                List<IBalance> balances = await repo.GetWallets();

                foreach (AssetPair assetPair in await dataAccess.GetAssetPairs())
                {
                    List<Price> historyPrices = await dataAccess.GetAssetPairHistory(assetPair.Id);
                    IPrice actualPrice = await repo.GetPrice(assetPair.Id);

                    if (actualPrice is not Price)
                    {
                        continue;
                    }

                    Price price = (Price)actualPrice;
                    historyPrices.Add(price);
                    DecisionAudit decisionAudit = _post2DaysDiffSlopeStrategy.advice(historyPrices, balances, assetPair, trades, price);
                    String reason = "unknown";

                    if (BUY.Equals(decisionAudit.Advice))
                    {
                        Decimal volume = CHF_TO_SPEND_AT_ONCE / price.Ask;
                        _logger.LogInformation("Should buy: {0}, volume: {1}", assetPair.Id, volume);
                        IResponse<String> response = await repo.LimitOrderBuy(assetPair.Id, Decimal.Round(price.Ask, assetPair.PriceAccuracy), Decimal.Round(volume, assetPair.QuoteAssetAccuracy));
                        if (response.IsSuccess())
                        {
                            reason = "buy orderId: " + await response.GetResponse();
                            balances = await repo.GetWallets();
                        }
                        else
                        {
                            reason = "ReasonOfFailure: " + response.GetReasonOfFailure() + ", Errormessage: " + response.GetErrorMessage();
                        }
                    }

                    else if (SELL.Equals(decisionAudit.Advice))
                    {
                        decimal baseAssetToSell = getMaxVolumeToSellAlwaysWin(trades, balances, assetPair, price);
                        _logger.LogInformation("Should sell: {0}, volume: {1}", assetPair.Id, baseAssetToSell);
                        IResponse<String> response = await repo.LimitOrderSell(assetPair.Id, Decimal.Round(price.Ask, assetPair.PriceAccuracy), Decimal.Round(baseAssetToSell, assetPair.BaseAssetAccuracy));
                        if (response.IsSuccess())
                        {
                            reason = "sell orderId: " + await response.GetResponse();
                            balances = await repo.GetWallets();
                        }
                        else
                        {
                            reason = "ReasonOfFailure: " + response.GetReasonOfFailure() + ", Errormessage: " + response.GetErrorMessage();
                        }
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

        public decimal getMaxVolumeToSellAlwaysWin(List<TradeEntry> trades, List<IBalance> balances, AssetPair assetPair, Price price)
        {
            IBalance baseAssetBalance = balances.Where(b => assetPair.BaseAssetId.Equals(b.AssetId)).FirstOrDefault(new Balance("n/a", 0, 0));
            Decimal baseAssetMaxAvailableAmountToSell = baseAssetBalance.Available - baseAssetBalance.Reserved;

            List<TradeEntry> tradeEntriesOfAsset = trades.Where(trade => trade.baseAssetId.Equals(assetPair.BaseAssetId)).ToList();
            // Sort the entries from newest first to oldest
            tradeEntriesOfAsset.Sort((a, b) => b.timestamp.CompareTo(a.timestamp));
            List<TradeEntry> buyTradesToConsider = new List<TradeEntry>();

            Decimal soldVolume = 0;
            Decimal soldPrice = 0;

            foreach (TradeEntry trade in tradeEntriesOfAsset)
            {
                if (trade.side.Equals("buy"))
                {
                    if (soldVolume > 0 && trade.price < soldPrice)
                    {
                        soldVolume -= trade.baseVolume;
                        if (soldVolume <= 0)
                        {
                            soldPrice = 0;
                        }
                    }
                    else
                    {
                        buyTradesToConsider.Add(trade);
                    }
                }
                else if (trade.side.Equals("sell"))
                {
                    if (soldPrice > 0)
                    {
                        soldPrice = Math.Max(soldPrice, trade.price);
                    }
                    else
                    {
                        soldPrice = trade.price;
                    }
                    soldVolume += trade.baseVolume;
                }
            }

            Decimal baseAssetSellIfBoughtForLess = getAllBaseAssetVolumeIfBoughtLowerThanActualPrice(price, buyTradesToConsider, assetPair.BaseAssetId);
            return Math.Min(baseAssetMaxAvailableAmountToSell, baseAssetSellIfBoughtForLess);
        }

        private decimal getAllBaseAssetVolumeIfBoughtLowerThanActualPrice(Price price, List<TradeEntry> trades, string baseAssetId)
        {
            return trades.Where(trade => trade.baseAssetId.Equals(baseAssetId) && trade.side.Equals("buy") && trade.price < price.Ask).Sum(trade => trade.baseVolume);
        }

        private async Task RefreshHistory()
        {
            using (var scope = _scopeFactory.CreateScope())
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
            return Task.CompletedTask;
        }
    }
}