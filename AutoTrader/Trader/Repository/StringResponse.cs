using System;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public class StringResponse : IResponse<String>
    {
        Task<String> response;
        Boolean success;
        ReasonOfFailure reasonOfFailure;

        String errorMessage;

        public StringResponse(Task<String> response, bool success, ReasonOfFailure reasonOfFailure, string errorMessage)
        {
            this.response = response;
            this.success = success;
            this.reasonOfFailure = reasonOfFailure;
            this.errorMessage = errorMessage;
        }

        public StringResponse(Task<String> response)
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

        public Task<String> GetResponse()
        {
            return response;
        }

        public bool IsSuccess()
        {
            return success;
        }

        public string GetErrorMessage()
        {
            return errorMessage;
        }
    }
}
