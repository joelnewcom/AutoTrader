using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AutoTrader.Repository
{
    public class RawResponseRepository : IRepositoryGen<Task<IResponse>>
    {

        private readonly ILogger<RawResponseRepository> _logger;

        private IRepositoryGen<Task<HttpResponseMessage>> lykkeRepository;

        public RawResponseRepository(ILogger<RawResponseRepository> logger, IRepositoryGen<Task<HttpResponseMessage>> lykkeRepositoryBase)
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

        public async Task<IResponse> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            Task<HttpResponseMessage> task = lykkeRepository.GetHistoryRatePerDay(assetPairId, date);
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();

            if (response.Contains("API calls quota exceeded"))
            {
                _logger.LogWarning("API calls quota exceeded");
                return new WrappedResponse(task, false, ReasonOfFailure.QuotaExceeded);
            }

            return new WrappedResponse(task);
        }

        public async Task<IResponse> GetWallets()
        {
            return new WrappedResponse(lykkeRepository.GetWallets());
        }
    }
}
