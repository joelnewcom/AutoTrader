using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoTrader.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assetPairEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accuracy = table.Column<int>(type: "int", nullable: false),
                    BaseAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuotingAssetId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetPairEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "priceEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetPairId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ask = table.Column<float>(type: "real", nullable: false),
                    Bid = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_priceEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assetPairEntities");

            migrationBuilder.DropTable(
                name: "priceEntities");
        }
    }
}
