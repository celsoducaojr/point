using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class StockHistoryType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockItemId",
                table: "StockHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "StockHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_StockItemId",
                table: "StockHistories",
                column: "StockItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistories_StockItems_StockItemId",
                table: "StockHistories",
                column: "StockItemId",
                principalTable: "StockItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistories_StockItems_StockItemId",
                table: "StockHistories");

            migrationBuilder.DropIndex(
                name: "IX_StockHistories_StockItemId",
                table: "StockHistories");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "StockHistories");

            migrationBuilder.AlterColumn<int>(
                name: "StockItemId",
                table: "StockHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
