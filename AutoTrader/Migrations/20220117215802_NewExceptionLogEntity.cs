using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoTrader.Migrations
{
    public partial class NewExceptionLogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "exceptionLogEnities",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_exceptionLogEnities",
                table: "exceptionLogEnities",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_exceptionLogEnities",
                table: "exceptionLogEnities");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "exceptionLogEnities");
        }
    }
}
