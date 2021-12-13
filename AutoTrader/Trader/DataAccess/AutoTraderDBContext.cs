using System;
using AutoTrader.Data;
using Microsoft.EntityFrameworkCore;

public class AutoTraderDBContext : DbContext
{
    public string DbPath { get; }
    public AutoTraderDBContext(DbContextOptions<AutoTraderDBContext> options) : base(options)
    {
    }

    public DbSet<PriceEntity> priceEntities { get; set; }

    public DbSet<AssetPairEntity> assetPairEntities { get; set;}

}