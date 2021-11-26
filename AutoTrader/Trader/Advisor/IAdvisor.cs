using System.Threading.Tasks;

namespace AutoTrader.Advisor
{
    public interface IAdvisor<T>
    {
        Advice advice(T dataIn);
    }

    public interface IAsyncAdvisor<T>
    {
        Task<Advice> advice(T dataIn);
    }
}
