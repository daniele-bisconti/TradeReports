using Microsoft.EntityFrameworkCore.Migrations;

namespace TradeReports.Core.Migrations
{
    public partial class initrenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations");

            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Pos_PosId",
                table: "Operations");

            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Tools_ToolId",
                table: "Operations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tools",
                table: "Tools");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operations",
                table: "Operations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Tools",
                newName: "Tool");

            migrationBuilder.RenameTable(
                name: "Operations",
                newName: "Operation");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Tools_CategoryId",
                table: "Tool",
                newName: "IX_Tool_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_ToolId",
                table: "Operation",
                newName: "IX_Operation_ToolId");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_PosId",
                table: "Operation",
                newName: "IX_Operation_PosId");

            migrationBuilder.RenameIndex(
                name: "IX_Operations_CategoryId",
                table: "Operation",
                newName: "IX_Operation_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tool",
                table: "Tool",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operation",
                table: "Operation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_Category_CategoryId",
                table: "Operation",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_Pos_PosId",
                table: "Operation",
                column: "PosId",
                principalTable: "Pos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operation_Tool_ToolId",
                table: "Operation",
                column: "ToolId",
                principalTable: "Tool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tool_Category_CategoryId",
                table: "Tool",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Category_CategoryId",
                table: "Operation");

            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Pos_PosId",
                table: "Operation");

            migrationBuilder.DropForeignKey(
                name: "FK_Operation_Tool_ToolId",
                table: "Operation");

            migrationBuilder.DropForeignKey(
                name: "FK_Tool_Category_CategoryId",
                table: "Tool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tool",
                table: "Tool");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operation",
                table: "Operation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Tool",
                newName: "Tools");

            migrationBuilder.RenameTable(
                name: "Operation",
                newName: "Operations");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Tool_CategoryId",
                table: "Tools",
                newName: "IX_Tools_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Operation_ToolId",
                table: "Operations",
                newName: "IX_Operations_ToolId");

            migrationBuilder.RenameIndex(
                name: "IX_Operation_PosId",
                table: "Operations",
                newName: "IX_Operations_PosId");

            migrationBuilder.RenameIndex(
                name: "IX_Operation_CategoryId",
                table: "Operations",
                newName: "IX_Operations_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tools",
                table: "Tools",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operations",
                table: "Operations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Pos_PosId",
                table: "Operations",
                column: "PosId",
                principalTable: "Pos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Tools_ToolId",
                table: "Operations",
                column: "ToolId",
                principalTable: "Tools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tools_Categories_CategoryId",
                table: "Tools",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
