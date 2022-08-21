using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoTrader.Migrations
{
    public partial class LogBookUseListInsteadOfString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logBookEntry",
                table: "logBooks");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "logBooks",
                newName: "Reason");

            migrationBuilder.CreateTable(
                name: "DecisionEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AdviceType = table.Column<int>(type: "INTEGER", nullable: false),
                    Advice = table.Column<int>(type: "INTEGER", nullable: false),
                    LogBookId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LogBookEntityId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionEntities_logBooks_LogBookEntityId",
                        column: x => x.LogBookEntityId,
                        principalTable: "logBooks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DecisionEntities_LogBookEntityId",
                table: "DecisionEntities",
                column: "LogBookEntityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DecisionEntities");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "logBooks",
                newName: "reason");

            migrationBuilder.AddColumn<string>(
                name: "logBookEntry",
                table: "logBooks",
                type: "TEXT",
                nullable: true);
        }
    }
}
