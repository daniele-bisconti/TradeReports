using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeReports.Core.Migrations
{
    public partial class addcapital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capital",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capital", x => x.Date);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capital");
        }
    }
}
