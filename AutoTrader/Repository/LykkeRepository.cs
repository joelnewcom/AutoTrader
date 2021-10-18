using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class LykkeRepository : IRepositoryGen<Task<HttpResponseMessage>>
    {
        private readonly String publicApi = "https://public-api.lykke.com";
        private readonly HttpClient client;
        private string applicationJson = "application/json";

        private readonly ILogger<LykkeRepository> _logger;

        public LykkeRepository(ILogger<LykkeRepository> logger)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
            _logger = logger;
        }

        public async Task<HttpResponseMessage> IsAliveAsync()
        {
            return  await client.GetAsync(publicApi + "/api/IsAlive");
        }

        public async Task<HttpResponseMessage> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            PayloadGetHistoryRate payload = new PayloadGetHistoryRate { Period = "Day", DateTime = date };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);

            return await client.PostAsync(publicApi + "/api/AssetPairs/rate/history/" + assetPairId, httpContent);
        }

        public async Task<HttpResponseMessage> GetAssetPairsDictionary()
        {
            return await client.GetAsync(publicApi + "/api/AssetPairs/dictionary/Spot");
        }
    }
}
