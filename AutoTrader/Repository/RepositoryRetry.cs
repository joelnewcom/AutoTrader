using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    public class RepositoryRetry : IRepository
    {
        private readonly ILogger<RepositoryRetry> _logger;

        private readonly int retries = 5;

        private readonly int delayBetweenReriesInSeconds = 10;

        private IRepositoryGen<Task<IResponse>> wrappedResponeRepo;

        public RepositoryRetry(ILogger<RepositoryRetry> logger, IRepositoryGen<Task<IResponse>> wrappedResponseRepo)
        {
            this.wrappedResponeRepo = wrappedResponseRepo;
            _logger = logger;
        }

        public async Task<Boolean> IsAliveAsync()
        {
            IResponse response = await wrappedResponeRepo.IsAliveAsync();
            HttpResponseMessage msg = await response.GetResponse();
            return msg.IsSuccessStatusCode;
        }

        public async Task<IAssetPairHistoryEntry> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            String responseString = "";
            foreach (int value in Enumerable.Range(1, retries))
            {
                IResponse response = await wrappedResponeRepo.GetHistoryRatePerDay(assetPairId, date);
                if (response.IsSuccess())
                {
                    HttpResponseMessage msg = await response.GetResponse();
                    responseString = await msg.Content.ReadAsStringAsync();
                    break;
                }
                await Task.Delay(delayBetweenReriesInSeconds * 1000);
            }

            try
            {
                PayloadResponseGetHistoryRate deserializeObject = JsonConvert.DeserializeObject<PayloadResponseGetHistoryRate>(responseString);
                if (deserializeObject is null)
                {
                    return new NoDataHistoryEntry();
                }

                return new AssetPairHistoryEntry(date, deserializeObject.Ask, deserializeObject.Bid);
            }

            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse history rate response of assetId" + assetPairId + " raw response: " + responseString);
                return new NoDataHistoryEntry();
            }
        }

        public async Task<Dictionary<string, AssetPair>> GetAssetPairsDictionary()
        {

            String responseString = "";
            foreach (int value in Enumerable.Range(1, retries))
            {
                IResponse response = await wrappedResponeRepo.GetAssetPairsDictionary();

                if (response.IsSuccess())
                {
                    HttpResponseMessage msg = await response.GetResponse();
                    responseString = await msg.Content.ReadAsStringAsync();
                    break;
                }
                await Task.Delay(delayBetweenReriesInSeconds * 1000);
            }

            List<PayloadAssetPairDict> deserializeObject = JsonConvert.DeserializeObject<List<PayloadAssetPairDict>>(responseString);

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
