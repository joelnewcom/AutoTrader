using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Data;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Repository
{
    public class WrappedResponseAdapter : IRepositoryGen<Task<IResponse>>
    {

        private readonly ILogger<WrappedResponseAdapter> _logger;

        private IRepositoryGen<Task<HttpResponseMessage>> lykkeRepository;

        public WrappedResponseAdapter(ILogger<WrappedResponseAdapter> logger, IRepositoryGen<Task<HttpResponseMessage>> lykkeRepositoryBase)
        {
            this.lykkeRepository = lykkeRepositoryBase;
            _logger = logger;
        }

        public async Task<IResponse> GetAssetPairsDictionary()
        {
            Task<HttpResponseMessage> task = lykkeRepository.GetAssetPairsDictionary();
            HttpResponseMessage msg = await task;
            return new WrappedResponse(task);
        }

        public async Task<IResponse> IsAliveAsync()
        {
            Task<HttpResponseMessage> task = lykkeRepository.IsAliveAsync();
            HttpResponseMessage msg = await task;
            return new WrappedResponse(task, msg.IsSuccessStatusCode, ReasonOfFailure.None);
        }

        public async Task<IResponse> GetHistoryRatePerDay(AssetPair assetPair, DateTime date)
        {
            Task<HttpResponseMessage> task = lykkeRepository.GetHistoryRatePerDay(assetPair, date);
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();

            if (response.Contains("API calls quota exceeded"))
            {
                _logger.LogWarning("API calls quota exceeded");
                return new WrappedResponse(task, false, ReasonOfFailure.QuotaExceeded);
            }

            return new WrappedResponse(task);
        }
    }
}
