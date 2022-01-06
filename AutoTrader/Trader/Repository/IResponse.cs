using System;
using System.Threading.Tasks;

namespace AutoTrader.Repository
{
    public interface IResponse<T>
    {
        Task<T> GetResponse(); 
        Boolean IsSuccess();
        ReasonOfFailure GetReasonOfFailure();
        String GetErrorMessage();
    }
}
