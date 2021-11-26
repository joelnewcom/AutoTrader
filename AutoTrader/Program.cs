using AutoTrader.Data;
using AutoTrader.Library;
using AutoTrader.Repository;
using AutoTrader.Trader;
using AutoTrader.Trader.Repository.Lykke.PocoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

namespace AutoTrader
{
    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
         Host.CreateDefaultBuilder(args)
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder.UseStartup<Startup>();
             })
             .ConfigureServices(services =>
             {
                 services.AddHostedService<TraderService>();
                 services.AddSingleton<IRepositoryGen<Task<HttpResponseMessage>>, LykkeRepository>();
                 services.AddSingleton<IRepositoryGen<Task<IResponse>>, RawResponseRepository>();
                 services.AddSingleton<IRepository, RetryRepository>();
                 services.AddSingleton<DataRefresher>();
                 services.AddSingleton<IDataAccess, DataInFile>();
                 services.AddSingleton<AssetPairHistoryEntryMapper>();
                 services.AddSingleton<TradeEntryMapper>();
                 services.AddSingleton<PriceMapper>();
                 services.AddMvcCore().AddApiExplorer();
                 services.AddSwaggerGen();
             });
    }
}
