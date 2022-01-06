using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public class HttpResponse : IResponse<HttpResponseMessage>
    {
        Task<HttpResponseMessage> response;
        Boolean success;
        ReasonOfFailure reasonOfFailure;

        String errorMessage;

        public HttpResponse(Task<HttpResponseMessage> response, bool success, ReasonOfFailure reasonOfFailure, string errorMessage)
        {
            this.response = response;
            this.success = success;
            this.reasonOfFailure = reasonOfFailure;
            this.errorMessage = errorMessage;
        }

        public HttpResponse(Task<HttpResponseMessage> response)
        {
            this.response = response;
            this.success = true;
            this.reasonOfFailure = ReasonOfFailure.None;
            this.errorMessage = "";
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

        public string GetErrorMessage()
        {
            throw new NotImplementedException();
        }
    }
}
