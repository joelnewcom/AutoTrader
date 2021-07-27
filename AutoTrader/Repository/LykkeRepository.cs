using AutoTrader.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoTrader.Data;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class LykkeRepository : IRepository
    {

        private String publicApi = "https://public-api.lykke.com";
        private readonly HttpClient Client;
        private string applicationJson = "application/json";

        public LykkeRepository(string publicApi)
        {
            this.publicApi = publicApi;
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(applicationJson));
        }

        public Dictionary<string, AssetPair> GetAssetPairsDictionary()
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> IsAliveAsync()
        {
            Task<HttpResponseMessage> task = Client.GetAsync(publicApi + "/api/IsAlive");
            HttpResponseMessage msg = await task;
            return msg.IsSuccessStatusCode;
        }

        public async Task<IAssetPairHistoryEntry> GetHistoryRatePerDay(AssetPair assetPair, DateTime date)
        {
            PayloadGetHistoryRate payload = new PayloadGetHistoryRate {Period = "Day", DateTime = date};

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, applicationJson);
            Task<HttpResponseMessage> task = Client.PostAsync(publicApi + "/api/AssetPairs/rate/history/" + assetPair.Id, httpContent);
            HttpResponseMessage msg = await task;
            String response = await msg.Content.ReadAsStringAsync();
            PayloadResponseGetHistoryRate deserializeObject = JsonConvert.DeserializeObject<PayloadResponseGetHistoryRate>(response);

            if (deserializeObject is null)
            {
                return new NoDataHistoryEntry();
            }

            return new AssetPairHistoryEntry(date, deserializeObject.Ask, deserializeObject.Bid);
        }

        Task<Dictionary<string, AssetPair>> IRepository.GetAssetPairsDictionary()
        {
            throw new NotImplementedException();
        }
    }
}
