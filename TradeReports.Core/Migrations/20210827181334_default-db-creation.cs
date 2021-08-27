using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeReports.Core.Migrations
{
    public partial class defaultdbcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Pos",
                columns: new[] { "Id", "Description" },
                values: new object[] { 1, "Long" });

            migrationBuilder.InsertData(
                table: "Pos",
                columns: new[] { "Id", "Description" },
                values: new object[] { 2, "Short" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Pos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Pos",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
