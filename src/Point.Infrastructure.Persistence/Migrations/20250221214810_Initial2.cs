using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Item_ItemId",
                table: "ItemTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Item_ItemId",
                table: "ItemTag",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemTag_Item_ItemId",
                table: "ItemTag");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTag_Item_ItemId",
                table: "ItemTag",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id");
        }
    }
}
