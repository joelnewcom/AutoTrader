using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public interface IResponse
    {
        Task<HttpResponseMessage> GetResponse(); 
        Boolean IsSuccess();

        ReasonOfFailure GetReasonOfFailure();
    }
}
