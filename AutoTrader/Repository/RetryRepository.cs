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
    // Takes the specific repository and transforms it into own domain language.
    public class RetryRepository : IRepository
    {
        private readonly ILogger<RetryRepository> _logger;

        private readonly int retries = 5;

        private readonly int delayBetweenReriesInSeconds = 10;

        private IRepositoryGen<Task<IResponse>> wrappedResponeRepo;

        public RetryRepository(ILogger<RetryRepository> logger, IRepositoryGen<Task<IResponse>> wrappedResponseRepo)
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

            Dictionary<String, AssetPair> assetPairs = new Dictionary<string, AssetPair>();

            if (deserializeObject is null)
            {
                return assetPairs;
            }

            foreach (PayloadAssetPairDict item in deserializeObject)
            {
                assetPairs.Add(item.id, new AssetPair(item.id, item.name, item.accuracy));
            }

            return assetPairs;
        }

        public async Task<List<IWalletEntry>> GetWallets()
        {
            IResponse reponse = await wrappedResponeRepo.GetWallets();
            HttpResponseMessage responseMessage = await reponse.GetResponse();

            PayloadWrapper<List<PayloadBalance>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadBalance>>>(await responseMessage.Content.ReadAsStringAsync());

            List<IWalletEntry> responseObjects = new List<IWalletEntry>();

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return responseObjects;
            }

            foreach (PayloadBalance item in deserializeObject.Payload)
            {
                responseObjects.Add(new WalletEntry(item.AssetId, item.Available, item.Reserved));
            }

            return responseObjects;
        }

        public async Task<List<TradeEntry>> GetTrades()
        {
            IResponse reponse = await wrappedResponeRepo.GetTrades();
            HttpResponseMessage responseMessage = await reponse.GetResponse();
            PayloadWrapper<List<PayloadTradeHistory>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadTradeHistory>>>(await responseMessage.Content.ReadAsStringAsync());
            List<TradeEntry> responseObjects = new List<TradeEntry>();
            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return responseObjects;
            }

            foreach (PayloadTradeHistory item in deserializeObject.Payload)
            {
                responseObjects.Add(new TradeEntry(item.Id, item.timestamp, item.assetPairId, item.price, item.baseAssetId, item.quoteAssetId));
            }

            return responseObjects;

        }
    }
}
