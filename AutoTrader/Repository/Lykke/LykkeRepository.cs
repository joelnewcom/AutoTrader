using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class LykkeRepository : IRepositoryGen<Task<HttpResponseMessage>>
    {
        private readonly String publicApi = "https://public-api.lykke.com";

        private readonly String privateApi = "https://hft-api.lykke.com";
        private readonly HttpClient httpClient;
        private string applicationJson = "application/json";

        private readonly String apiKey;

        private readonly ILogger<LykkeRepository> _logger;

        public LykkeRepository(ILogger<LykkeRepository> logger, TraderConfig traderConfig)
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            _logger = logger;
            this.apiKey = traderConfig.apiKey;
        }

        public async Task<HttpResponseMessage> IsAliveAsync()
        {
            return await httpClient.GetAsync(publicApi + "/api/IsAlive");
        }

        public async Task<HttpResponseMessage> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            PayloadGetHistoryRate payload = new PayloadGetHistoryRate { Period = "Day", DateTime = date };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);

            return await httpClient.PostAsync(publicApi + "/api/AssetPairs/rate/history/" + assetPairId, httpContent);
        }

        public async Task<HttpResponseMessage> GetAssetPairsDictionary()
        {
            return await httpClient.GetAsync(publicApi + "/api/AssetPairs/dictionary/Spot");
        }

        public async Task<HttpResponseMessage> GetWallets()
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, privateApi + "/api/Wallets");
            requestMessage.Headers.Add("api-key", apiKey);
            return await httpClient.SendAsync(requestMessage);
        }
    }
}
