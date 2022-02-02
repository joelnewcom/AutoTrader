using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Repository
{
    public class RawResponseRepository : IRepositoryGen<Task<IResponse<HttpResponseMessage>>>
    {

        private readonly ILogger<RawResponseRepository> _logger;

        private IRepositoryGen<Task<HttpResponseMessage>> _lykkeRepository;

        public RawResponseRepository(ILogger<RawResponseRepository> logger, IRepositoryGen<Task<HttpResponseMessage>> lykkeRepositoryBase)
        {
            _lykkeRepository = lykkeRepositoryBase;
            _logger = logger;
        }

        public Task<IResponse<HttpResponseMessage>> GetAssetPairsDictionary()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetAssetPairsDictionary()));
        }
        public Task<IResponse<HttpResponseMessage>> GetAssetPairs()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetAssetPairs()));
        }

        public async Task<IResponse<HttpResponseMessage>> IsAliveAsync()
        {
            Task<HttpResponseMessage> task = _lykkeRepository.IsAliveAsync();
            HttpResponseMessage msg = await task;
            return new HttpResponse(task, msg.IsSuccessStatusCode, ReasonOfFailure.None, await msg.Content.ReadAsStringAsync());
        }

        public async Task<IResponse<HttpResponseMessage>> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            Task<HttpResponseMessage> task = _lykkeRepository.GetHistoryRatePerDay(assetPairId, date);
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();

            if (response.Contains("API calls quota exceeded"))
            {
                _logger.LogWarning("API calls quota exceeded");
                return new HttpResponse(task, false, ReasonOfFailure.QuotaExceeded, await msg.Content.ReadAsStringAsync());
            }

            return new HttpResponse(task);
        }

        public Task<IResponse<HttpResponseMessage>> GetWallets()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetWallets()));
        }

        public Task<IResponse<HttpResponseMessage>> GetTrades()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetTrades()));
        }

        public Task<IResponse<HttpResponseMessage>> GetPrice(string assetPairId)
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetPrice(assetPairId)));
        }

        public Task<IResponse<HttpResponseMessage>> LimitOrderBuy(string assetPairId, Decimal price, Decimal volume)
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.LimitOrderBuy(assetPairId, price, volume)));
        }

        public Task<IResponse<HttpResponseMessage>> LimitOrderSell(string assetPairId, Decimal price, Decimal volume)
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.LimitOrderSell(assetPairId, price, volume)));
        }

        public Task<IResponse<HttpResponseMessage>> GetOperations()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetOperations()));
        }

        public Task<IResponse<HttpResponseMessage>> GetPrices()
        {
            return Task.FromResult<IResponse<HttpResponseMessage>>(new HttpResponse(_lykkeRepository.GetPrices()));
        }
    }
}
