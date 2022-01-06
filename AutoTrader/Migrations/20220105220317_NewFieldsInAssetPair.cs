using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoTrader.Migrations
{
    public partial class NewFieldsInAssetPair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Accuracy",
                table: "assetPairEntities",
                newName: "QuotingAssetAccuracy");

            migrationBuilder.AddColumn<int>(
                name: "BaseAssetAccuracy",
                table: "assetPairEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceAccuracy",
                table: "assetPairEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseAssetAccuracy",
                table: "assetPairEntities");

            migrationBuilder.DropColumn(
                name: "PriceAccuracy",
                table: "assetPairEntities");

            migrationBuilder.RenameColumn(
                name: "QuotingAssetAccuracy",
                table: "assetPairEntities",
                newName: "Accuracy");
        }
    }
}
