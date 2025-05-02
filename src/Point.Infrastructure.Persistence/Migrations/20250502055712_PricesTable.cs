using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PricesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Price_ItemUnits_ItemUnitId",
                table: "Price");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Price",
                table: "Price");

            migrationBuilder.RenameTable(
                name: "Price",
                newName: "Prices");

            migrationBuilder.RenameIndex(
                name: "IX_Price_ItemUnitId",
                table: "Prices",
                newName: "IX_Prices_ItemUnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prices",
                table: "Prices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_ItemUnits_ItemUnitId",
                table: "Prices",
                column: "ItemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_ItemUnits_ItemUnitId",
                table: "Prices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prices",
                table: "Prices");

            migrationBuilder.RenameTable(
                name: "Prices",
                newName: "Price");

            migrationBuilder.RenameIndex(
                name: "IX_Prices_ItemUnitId",
                table: "Price",
                newName: "IX_Price_ItemUnitId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Price",
                table: "Price",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Price_ItemUnits_ItemUnitId",
                table: "Price",
                column: "ItemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id");
        }
    }
}
