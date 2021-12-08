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
                 services.AddScoped<IRepositoryGen<Task<HttpResponseMessage>>, LykkeRepository>();
                 services.AddScoped<IRepositoryGen<Task<IResponse>>, RawResponseRepository>();
                 services.AddScoped<IRepository, RetryRepository>();
                 services.AddScoped<DataRefresher>();
                //  services.AddSingleton<IDataAccess, DataInFile>();
                 services.AddScoped<IDataAccessAsync, DataInDB>();
                 services.AddScoped<AssetPairHistoryEntryMapper>();
                 services.AddScoped<TradeEntryMapper>();
                 services.AddScoped<PriceMapper>();
                 services.AddScoped<AssetPairMapper>();
                 services.AddMvcCore().AddApiExplorer();
                 services.AddSwaggerGen();
             });
    }
}
