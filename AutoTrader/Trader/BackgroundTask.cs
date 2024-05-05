using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTrader.Config;
using AutoTrader.Data;
using AutoTrader.Library;
using AutoTrader.Models;
using AutoTrader.Repository;
using AutoTrader.Trader.Advisor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Trader;

public class BackgroundTask
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

    public BackgroundTask(ILogger<TraderService> logger, TraderConfig traderConfig, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _conf = traderConfig;
        _post2DaysDiffSlopeStrategy = new Post2DaysDiffSlopeStrategy(_conf.safetyCatch, CHF_TO_SPEND_AT_ONCE);
    }

    public async void DoWork(object stateInfo)
    {
        try
        {
            _logger.LogInformation("Lykke trader started to do work");
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
            Console.WriteLine("Time: {0}, InvokeCount:  {1,2}.", DateTime.Now.ToString("h:mm:ss.fff"),
                (++_invokeCount).ToString());
            await RefreshHistory();
            Trade();
        }
        catch (Exception e)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                IDataAccess dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();
                String exceptionLog =
                    await dataAccess.AddExceptionLog(new ExceptionLog(Guid.NewGuid(), e.Message, DateTime.Now));
                _logger.LogError("Catched unexpected exception {0}", exceptionLog);
            }
        }
    }

    private async void Trade()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            IRepository repo = scope.ServiceProvider.GetRequiredService<IRepository>();
            IDataAccess dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();
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
                Guid logBookId = Guid.NewGuid();
                DecisionAudit decisionAudit =
                    _post2DaysDiffSlopeStrategy.advice(historyPrices, balances, assetPair, trades, price, logBookId);
                String reason = "unknown";

                if (BUY.Equals(decisionAudit.Advice))
                {
                    Decimal volume = CHF_TO_SPEND_AT_ONCE / price.Ask;
                    _logger.LogInformation("Should buy: {0}, volume: {1}", assetPair.Id, volume);
                    IResponse<String> response = await repo.LimitOrderBuy(assetPair.Id,
                        Decimal.Round(price.Ask, assetPair.PriceAccuracy),
                        Decimal.Round(volume, assetPair.QuoteAssetAccuracy));
                    if (response.IsSuccess())
                    {
                        reason = "buy orderId: " + await response.GetResponse();
                        balances = await repo.GetWallets();
                    }
                    else
                    {
                        reason = "ReasonOfFailure: " + response.GetReasonOfFailure() + ", Errormessage: " +
                                 response.GetErrorMessage();
                    }
                }

                else if (SELL.Equals(decisionAudit.Advice))
                {
                    decimal baseAssetToSell = GetMaxVolumeToSellAlwaysWin(trades, balances, assetPair, price);
                    _logger.LogInformation("Should sell: {0}, volume: {1}", assetPair.Id, baseAssetToSell);
                    IResponse<String> response = await repo.LimitOrderSell(assetPair.Id,
                        Decimal.Round(price.Ask, assetPair.PriceAccuracy),
                        Decimal.Round(baseAssetToSell, assetPair.BaseAssetAccuracy));
                    if (response.IsSuccess())
                    {
                        reason = "sell orderId: " + await response.GetResponse();
                        balances = await repo.GetWallets();
                    }
                    else
                    {
                        reason = "ReasonOfFailure: " + response.GetReasonOfFailure() + ", Errormessage: " +
                                 response.GetErrorMessage();
                    }
                }
                else
                {
                    _logger.LogInformation("Should hold on: " + assetPair.Id);
                    reason = "Hold on";
                }

                await dataAccess.AddLogBook(new LogBook(logBookId, assetPair.Id, DateTime.Now, reason,
                    decisionAudit.Decisions));
            }
        }
    }

    public decimal GetMaxVolumeToSellAlwaysWin(List<TradeEntry> trades, List<IBalance> balances, AssetPair assetPair,
        Price price)
    {
        IBalance baseAssetBalance = balances.Where(b => assetPair.BaseAssetId.Equals(b.AssetId))
            .FirstOrDefault(new Balance("n/a", 0, 0));
        Decimal baseAssetMaxAvailableAmountToSell = baseAssetBalance.Available - baseAssetBalance.Reserved;

        List<TradeEntry> tradeEntriesOfAsset =
            trades.Where(trade => trade.baseAssetId.Equals(assetPair.BaseAssetId)).ToList();
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

        Decimal baseAssetSellIfBoughtForLess =
            getAllBaseAssetVolumeIfBoughtLowerThanActualPrice(price, buyTradesToConsider, assetPair.BaseAssetId);
        return Math.Min(baseAssetMaxAvailableAmountToSell, baseAssetSellIfBoughtForLess);
    }

    private decimal getAllBaseAssetVolumeIfBoughtLowerThanActualPrice(Price price, List<TradeEntry> trades,
        string baseAssetId)
    {
        return trades
            .Where(trade =>
                trade.baseAssetId.Equals(baseAssetId) && trade.side.Equals("buy") && trade.price < price.Ask)
            .Sum(trade => trade.baseVolume);
    }

    private async Task RefreshHistory()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            DataRefresher dataRefresher = scope.ServiceProvider.GetRequiredService<DataRefresher>();
            IDataAccess dataAccess = scope.ServiceProvider.GetRequiredService<IDataAccess>();

            foreach (AssetPair assetPair in await dataAccess.GetAssetPairs())
            {
                await dataRefresher.RefreshAssetPairHistory(assetPair.Id);
            }
        }
    }
}