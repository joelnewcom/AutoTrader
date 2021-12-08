using AutoTrader.Data;
using Microsoft.EntityFrameworkCore;

public class AutoTraderDBContext : DbContext
{
    public AutoTraderDBContext(DbContextOptions<AutoTraderDBContext> options) : base(options)
    {

    }

    public DbSet<PriceEntity> priceEntities { get; set; }

    public DbSet<AssetPairEntity> assetPairEntities { get; set;}

}