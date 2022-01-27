using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTrader.Data;
using AutoTrader.Trader.PoCos;
using AutoTrader.Trader.Repository;
using AutoTrader.Trader.Repository.Lykke.PocoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AutoTrader.Repository
{
    // Takes the specific repository and transforms it into own domain language.
    public class BusinessDomainRepository : IRepository
    {
        private readonly ILogger<BusinessDomainRepository> _logger;
        private readonly int _retries = 5;
        private readonly int _delayBetweenReriesInSeconds = 10;
        private IRepositoryGen<Task<IResponse<HttpResponseMessage>>> _wrappedResponeRepo;
        private AssetPairHistoryEntryMapper _assetPairHistoryEntryMapper;
        private TradeEntryMapper _tradeEntryMapper;
        private PriceMapper _priceMapper;
        private AssetPairMapper _assetPairMapper;
        private OperationMapper _operationMapper;

        public BusinessDomainRepository(
            ILogger<BusinessDomainRepository> logger,
            IRepositoryGen<Task<IResponse<HttpResponseMessage>>> wrappedResponseRepo,
            AssetPairHistoryEntryMapper assetPairHistoryEntryMapper,
            TradeEntryMapper tradeEntryMapper,
            PriceMapper priceMapper,
            AssetPairMapper assetPairMapper,
            OperationMapper operationMapper)
        {
            _logger = logger;
            _wrappedResponeRepo = wrappedResponseRepo;
            _assetPairHistoryEntryMapper = assetPairHistoryEntryMapper;
            _tradeEntryMapper = tradeEntryMapper;
            _priceMapper = priceMapper;
            _assetPairMapper = assetPairMapper;
            _operationMapper = operationMapper;
        }

        public async Task<Boolean> IsAliveAsync()
        {
            IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.IsAliveAsync();
            HttpResponseMessage msg = await response.GetResponse();
            return msg.IsSuccessStatusCode;
        }

        public async Task<IPrice> GetHistoryRatePerDay(String assetPairId, DateTime date)
        {
            String responseString = "";
            foreach (int value in Enumerable.Range(1, _retries))
            {
                IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.GetHistoryRatePerDay(assetPairId, date);
                if (response.IsSuccess())
                {
                    HttpResponseMessage msg = await response.GetResponse();
                    responseString = await msg.Content.ReadAsStringAsync();
                    break;
                }
                await Task.Delay(_delayBetweenReriesInSeconds * 1000);
            }

            try
            {
                PayloadHistoryRate deserializeObject = JsonConvert.DeserializeObject<PayloadHistoryRate>(responseString);
                if (deserializeObject is null)
                {
                    return new NoDataPrice();
                }

                return _assetPairHistoryEntryMapper.create(deserializeObject, date);
            }

            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse history rate response of assetId: " + assetPairId + " raw response: " + responseString);
                return new NoDataPrice();
            }
        }

        public async Task<Dictionary<string, AssetPair>> GetAssetPairs()
        {

            String responseString = "";
            foreach (int value in Enumerable.Range(1, _retries))
            {
                IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.GetAssetPairs();

                if (response.IsSuccess())
                {
                    HttpResponseMessage msg = await response.GetResponse();
                    responseString = await msg.Content.ReadAsStringAsync();
                    break;
                }
                await Task.Delay(_delayBetweenReriesInSeconds * 1000);
            }

            PayloadWrapper<List<PayloadAssetPair>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadAssetPair>>>(responseString);

            Dictionary<String, AssetPair> assetPairs = new Dictionary<string, AssetPair>();

            if (deserializeObject is null)
            {
                return assetPairs;
            }

            foreach (PayloadAssetPair item in deserializeObject.Payload)
            {
                assetPairs.Add(item.assetPairId, _assetPairMapper.create(item));
            }

            return assetPairs;
        }

        public async Task<List<IBalance>> GetWallets()
        {
            IResponse<HttpResponseMessage> reponse = await _wrappedResponeRepo.GetWallets();
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
            IResponse<HttpResponseMessage> reponse = await _wrappedResponeRepo.GetTrades();
            HttpResponseMessage responseMessage = await reponse.GetResponse();

            try
            {
                PayloadWrapper<List<PayloadTradeHistory>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadTradeHistory>>>(await responseMessage.Content.ReadAsStringAsync());
                List<TradeEntry> responseObjects = new List<TradeEntry>();
                if (deserializeObject is null || deserializeObject.Payload is null)
                {
                    return responseObjects;
                }

                foreach (PayloadTradeHistory item in deserializeObject.Payload)
                {
                    responseObjects.Add(_tradeEntryMapper.build(item));
                }

                return responseObjects;
            }
            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse trade history response. Raw response: " + await responseMessage.Content.ReadAsStringAsync());
                return new List<TradeEntry>();
            }
        }

        public async Task<IPrice> GetPrice(string assetPairId)
        {
            IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.GetPrice(assetPairId);
            HttpResponseMessage responseMessage = await response.GetResponse();

            try
            {
                PayloadWrapper<List<PayloadPrice>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadPrice>>>(await responseMessage.Content.ReadAsStringAsync());
                if (deserializeObject is null || deserializeObject.Payload is null || deserializeObject.Payload.Count != 1)
                {
                    return new NoDataPrice();
                }

                return _priceMapper.build(deserializeObject.Payload.Last());
            }

            catch (JsonSerializationException)
            {
                _logger.LogWarning("Not able to parse price response of assetId: " + assetPairId + " raw response: " + await responseMessage.Content.ReadAsStringAsync());
                return new NoDataPrice();
            }
        }

        public async Task<IResponse<string>> LimitOrderBuy(string assetPairId, Decimal price, Decimal volume)
        {
            IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.LimitOrderBuy(assetPairId, price, volume);

            if (!response.IsSuccess())
            {
                return new StringResponse(Task.FromResult(""), false, ReasonOfFailure.Unknownfailure, response.GetErrorMessage());
            }

            HttpResponseMessage responseMessage = await response.GetResponse();
            PayloadWrapper<PayloadOrderId> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<PayloadOrderId>>(await responseMessage.Content.ReadAsStringAsync());

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return new StringResponse(Task.FromResult(""), false, ReasonOfFailure.Unknownfailure, "ErrorCode " + responseMessage.StatusCode + " LimitOrder Buy did not work due to " + await responseMessage.Content.ReadAsStringAsync());
            }

            return new StringResponse(Task.FromResult(deserializeObject.Payload.orderId));
        }

        public async Task<IResponse<string>> LimitOrderSell(string assetPairId, Decimal price, Decimal volume)
        {
            IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.LimitOrderSell(assetPairId, price, volume);

            if (!response.IsSuccess())
            {
                return new StringResponse(Task.FromResult(""), false, ReasonOfFailure.Unknownfailure, response.GetErrorMessage());
            }


            HttpResponseMessage responseMessage = await response.GetResponse();
            PayloadWrapper<PayloadOrderId> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<PayloadOrderId>>(await responseMessage.Content.ReadAsStringAsync());

            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return new StringResponse(Task.FromResult(""), false, ReasonOfFailure.Unknownfailure, "ErrorCode " + responseMessage.StatusCode + " LimitOrder Sell did not work due to " + await responseMessage.Content.ReadAsStringAsync());
            }

            return new StringResponse(Task.FromResult(deserializeObject.Payload.orderId));
        }

        public async Task<List<Operation>> GetOperations()
        {
            IResponse<HttpResponseMessage> response = await _wrappedResponeRepo.GetOperations();
            HttpResponseMessage responseMessage = await response.GetResponse();

            PayloadWrapper<List<PayloadOperation>> deserializeObject = JsonConvert.DeserializeObject<PayloadWrapper<List<PayloadOperation>>>(await responseMessage.Content.ReadAsStringAsync());
            List<Operation> responseObjects = new List<Operation>();
            if (deserializeObject is null || deserializeObject.Payload is null)
            {
                return responseObjects;
            }

            foreach (PayloadOperation item in deserializeObject.Payload)
            {
                responseObjects.Add(_operationMapper.create(item));
            }

            return responseObjects;
        }
    }
}
