using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeReports.Core.Migrations
{
    public partial class add_pos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PosId",
                table: "Operations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_PosId",
                table: "Operations",
                column: "PosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Pos_PosId",
                table: "Operations",
                column: "PosId",
                principalTable: "Pos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Pos_PosId",
                table: "Operations");

            migrationBuilder.DropTable(
                name: "Pos");

            migrationBuilder.DropIndex(
                name: "IX_Operations_PosId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "PosId",
                table: "Operations");
        }
    }
}
