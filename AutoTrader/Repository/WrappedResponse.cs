using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public class WrappedResponse : IResponse
    {
        Task<HttpResponseMessage> response;
        Boolean success;
        ReasonOfFailure reasonOfFailure;

        public WrappedResponse(Task<HttpResponseMessage> response, bool success, ReasonOfFailure reasonOfFailure)
        {
            this.response = response;
            this.success = success;
            this.reasonOfFailure = reasonOfFailure;
        }

        public WrappedResponse(Task<HttpResponseMessage> response)
        {
            this.response = response;
            this.success = true;
            this.reasonOfFailure = ReasonOfFailure.None;
        }

        public ReasonOfFailure GetReasonOfFailure()
        {
            return reasonOfFailure;
        }

        public Task<HttpResponseMessage> GetResponse()
        {
            return response;
        }

        public bool IsSuccess()
        {
            return success;
        }
    }
}
