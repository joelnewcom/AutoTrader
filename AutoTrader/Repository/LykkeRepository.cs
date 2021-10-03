using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class LykkeRepository : IRepository
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

        public async Task<Boolean> IsAliveAsync()
        {
            Task<HttpResponseMessage> task = client.GetAsync(publicApi + "/api/IsAlive");
            HttpResponseMessage msg = await task;
            return msg.IsSuccessStatusCode;
        }

        public async Task<IAssetPairHistoryEntry> GetHistoryRatePerDay(AssetPair assetPair, DateTime date)
        {
            PayloadGetHistoryRate payload = new PayloadGetHistoryRate { Period = "Day", DateTime = date };
            string content = JsonConvert.SerializeObject(payload);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, applicationJson);

            Task<HttpResponseMessage> task = client.PostAsync(publicApi + "/api/AssetPairs/rate/history/" + assetPair.Id, httpContent);
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();
            try
            {
                PayloadResponseGetHistoryRate deserializeObject = JsonConvert.DeserializeObject<PayloadResponseGetHistoryRate>(response);
                if (deserializeObject is null)
                {
                    return new NoDataHistoryEntry();
                }

                return new AssetPairHistoryEntry(date, deserializeObject.Ask, deserializeObject.Bid);
            }

            catch (Exception ex)
            {
                if (ex is JsonReaderException ||
                    ex is JsonSerializationException
                )
                {
                    _logger.LogWarning("Not able to parse history rate response of assetId" + assetPair.Id + " raw response: " + response);
                    return new NoDataHistoryEntry();
                }
                throw;
            }
        }

        public async Task<Dictionary<string, AssetPair>> GetAssetPairsDictionary()
        {
            Task<HttpResponseMessage> task = client.GetAsync(publicApi + "/api/AssetPairs/dictionary/Spot");
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();

            List<PayloadAssetPairDict> deserializeObject = JsonConvert.DeserializeObject<List<PayloadAssetPairDict>>(response);

            if (deserializeObject is null)
            {
                return new Dictionary<string, AssetPair>();
            }

            Dictionary<String, AssetPair> assetPairs = new Dictionary<string, AssetPair>();

            foreach (PayloadAssetPairDict item in deserializeObject)
            {
                assetPairs.Add(item.id, new AssetPair(item.id, item.name, item.accuracy));
            }

            return assetPairs;
        }
    }
}
