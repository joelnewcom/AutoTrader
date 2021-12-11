using System;
using AutoTrader.Data;
using Microsoft.EntityFrameworkCore;

public class AutoTraderDBContext : DbContext
{
    public string DbPath { get; }
    public AutoTraderDBContext(DbContextOptions<AutoTraderDBContext> options) : base(options)
    {
            // var folder = Environment.SpecialFolder.LocalApplicationData;
            // var path = Environment.GetFolderPath(folder);
            // DbPath = System.IO.Path.Join(path, "autoTrader.db");

    }

    public DbSet<PriceEntity> priceEntities { get; set; }

    public DbSet<AssetPairEntity> assetPairEntities { get; set;}


        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        //     => options.UseSqlite($"Data Source={DbPath}");

}