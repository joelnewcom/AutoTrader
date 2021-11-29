using System.Threading.Tasks;

namespace AutoTrader.Advisor
{
    public interface IAdvisor<T>
    {
        Advice advice(T dataIn);
    }

    public interface IAdvisor<T, T2>
    {
        Advice advice(T dataIn, T2 dataIn2);
    }

    public interface IAdvisor<T, T2, T3>
    {
        Advice advice(T dataIn, T2 dataIn2, T3 dataIn3);
    }

    public interface IAsyncAdvisor<T>
    {
        Task<Advice> advice(T dataIn);
    }
}
