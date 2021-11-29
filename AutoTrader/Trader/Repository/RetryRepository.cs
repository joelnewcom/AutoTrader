using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Trader.Repository.Lykke.PocoMapper;
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

        private AssetPairHistoryEntryMapper assetPairHistoryEntryMapper;
        private TradeEntryMapper tradeEntryMapper;
        private PriceMapper priceMapper;

        private AssetPairMapper assetPairMapper;

        public RetryRepository(
            ILogger<RetryRepository> logger,
            IRepositoryGen<Task<IResponse>> wrappedResponseRepo,
            AssetPairHistoryEntryMapper assetPairHistoryEntryMapper,
            TradeEntryMapper tradeEntryMapper,
            PriceMapper priceMapper,
            AssetPairMapper assetPairMapper)
        {
            _logger = logger;
            this.wrappedResponeRepo = wrappedResponseRepo;
            this.assetPairHistoryEntryMapper = assetPairHistoryEntryMapper;
            this.tradeEntryMapper = tradeEntryMapper;
            this.priceMapper = priceMapper;
            this.assetPairMapper = assetPairMapper;
        }

        public async Task<Boolean> IsAliveAsync()
        {
            IResponse response = await wrappedResponeRepo.IsAliveAsync();
            HttpResponseMessage msg = await response.GetResponse();
            return msg.IsSuccessStatusCode;
        }

        public async Task<IPrice> GetHistoryRatePerDay(String assetPairId, DateTime date)
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
                    return new NoDataPrice();
                }

                return assetPairHistoryEntryMapper.create(deserializeObject, date);
            }

            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse history rate response of assetId: " + assetPairId + " raw response: " + responseString);
                return new NoDataPrice();
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

            List<PayloadAssetPairDictEntry> deserializeObject = JsonConvert.DeserializeObject<List<PayloadAssetPairDictEntry>>(responseString);

            Dictionary<String, AssetPair> assetPairs = new Dictionary<string, AssetPair>();

            if (deserializeObject is null)
            {
                return assetPairs;
            }

            foreach (PayloadAssetPairDictEntry item in deserializeObject)
            {
                assetPairs.Add(item.id, assetPairMapper.create(item));
            }

            return assetPairs;
        }

        public async Task<List<IBalance>> GetWallets()
        {
            IResponse reponse = await wrappedResponeRepo.GetWallets();
            HttpResponseMessage responseMessage = await reponse.GetResponse();

            PayloadWrapper<List<PayloadBalance>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadBalance>>>(await responseMessage.Content.ReadAsStringAsync());

            List<IBalance> responseObjects = new List<IBalance>();

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return responseObjects;
            }

            foreach (PayloadBalance item in deserializeObject.Payload)
            {
                responseObjects.Add(new Balance(item.AssetId, item.Available, item.Reserved));
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
                responseObjects.Add(tradeEntryMapper.build(item));
            }

            return responseObjects;
        }

        public async Task<IPrice> GetPrice(string assetPairId)
        {
            IResponse response = await wrappedResponeRepo.GetPrice(assetPairId);
            HttpResponseMessage responseMessage = await response.GetResponse();

            try{
                PayloadWrapper<List<PayloadPrice>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadPrice>>>(await responseMessage.Content.ReadAsStringAsync());
                if (deserializeObject is null || deserializeObject.Payload is null || deserializeObject.Payload.Count != 1)
                {
                    return new NoDataPrice();
                }

                return priceMapper.build(deserializeObject.Payload.Last());
            }

            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse price response of assetId: " + assetPairId + " raw response: " + await responseMessage.Content.ReadAsStringAsync());
                return new NoDataPrice();
            }
        }

        public async Task<string> LimitOrderBuy(string assetPairId, float price, float volume)
        {
            IResponse response = await wrappedResponeRepo.LimitOrderBuy(assetPairId, price, volume);
            HttpResponseMessage responseMessage = await response.GetResponse();
            PayloadWrapper<PayloadOrderId> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<PayloadOrderId>>(await responseMessage.Content.ReadAsStringAsync());

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                throw new HttpRequestException("ErrorCode " + responseMessage.StatusCode + " LimitOrder Buy did not work due to " + responseMessage.Content.ReadAsStringAsync());
            }

            return deserializeObject.Payload.orderId;
        }

        public async Task<string> LimitOrderSell(string assetPairId, float price, float volume)
        {
            IResponse response = await wrappedResponeRepo.LimitOrderSell(assetPairId, price, volume);
            HttpResponseMessage responseMessage = await response.GetResponse();
            PayloadWrapper<PayloadOrderId> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<PayloadOrderId>>(await responseMessage.Content.ReadAsStringAsync());

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                throw new HttpRequestException("ErrorCode " + responseMessage.StatusCode + " LimitOrder Sell did not work due to " + responseMessage.Content.ReadAsStringAsync());
            }

            return deserializeObject.Payload.orderId;

        }
    }
}
