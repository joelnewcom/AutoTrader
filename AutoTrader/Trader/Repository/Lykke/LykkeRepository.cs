using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class LykkeRepository : IRepositoryGen<Task<HttpResponseMessage>>
    {
        private const String PUBLIC_API = "https://public-api.lykke.com";

        private const String PRIVATE_API = "https://hft-api.lykke.com";

        private const String LYKKE_API_V2 = "https://hft-apiv2.lykke.com";
        private readonly HttpClient httpClient;
        private string applicationJson = "application/json";

        private readonly String apiKey;

        private readonly ILogger<LykkeRepository> _logger;

        public LykkeRepository(ILogger<LykkeRepository> logger, TraderConfig traderConfig)
        {
            this.apiKey = traderConfig.apiKey;
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            _logger = logger;
        }

        public Task<HttpResponseMessage> IsAliveAsync()
        {
            return httpClient.GetAsync(PUBLIC_API + "/api/IsAlive");
        }

        public Task<HttpResponseMessage> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            PayloadGetHistoryRate payload = new PayloadGetHistoryRate { Period = "Day", DateTime = date };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);

            return httpClient.PostAsync(PUBLIC_API + "/api/AssetPairs/rate/history/" + assetPairId, httpContent);
        }

        public Task<HttpResponseMessage> GetAssetPairsDictionary()
        {
            return httpClient.GetAsync(PUBLIC_API + "/api/AssetPairs/dictionary/Spot");
        }
        public Task<HttpResponseMessage> GetAssetPairs()
        {
            return httpClient.GetAsync(LYKKE_API_V2 + "/api/assetpairs");
        }

        public Task<HttpResponseMessage> GetWallets()
        {
            return httpClient.GetAsync(LYKKE_API_V2 + "/api/balance");
        }

        public Task<HttpResponseMessage> GetTrades()
        {
            return httpClient.GetAsync(LYKKE_API_V2 + "/api/trades");
        }

        public Task<HttpResponseMessage> GetPrice(string assetPairId)
        {
            return httpClient.GetAsync(LYKKE_API_V2 + "/api/prices?assetPairIds=" + assetPairId);
        }

        public Task<HttpResponseMessage> LimitOrderBuy(String assetPairId, Decimal price, Decimal volume)
        {
            PayloadLimitOrder payload = new PayloadLimitOrder { AssetPairId = assetPairId, Price = price, Side = "buy", Volume = volume };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);
            return httpClient.PostAsync(LYKKE_API_V2 + "/api/orders/limit", httpContent);
        }

        public Task<HttpResponseMessage> LimitOrderSell(String assetPairId, Decimal price, Decimal volume)
        {
            PayloadLimitOrder payload = new PayloadLimitOrder { AssetPairId = assetPairId, Price = price, Side = "sell", Volume = volume };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);
            return httpClient.PostAsync(LYKKE_API_V2 + "/api/orders/limit", httpContent);
        }

        public Task<HttpResponseMessage> GetOperations()
        {
            return httpClient.GetAsync(LYKKE_API_V2 + "/api/operations");
        }
    }
}
