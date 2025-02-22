using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Cost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountVariation_PurchaseCost_PurchaseCostId",
                table: "DiscountVariation");

            migrationBuilder.DropTable(
                name: "PurchaseCost");

            migrationBuilder.RenameColumn(
                name: "PurchaseCostId",
                table: "DiscountVariation",
                newName: "CostId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountVariation_PurchaseCostId",
                table: "DiscountVariation",
                newName: "IX_DiscountVariation_CostId");

            migrationBuilder.CreateTable(
                name: "Cost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cost_ItemUnit_Id",
                        column: x => x.Id,
                        principalTable: "ItemUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountVariation_Cost_CostId",
                table: "DiscountVariation",
                column: "CostId",
                principalTable: "Cost",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountVariation_Cost_CostId",
                table: "DiscountVariation");

            migrationBuilder.DropTable(
                name: "Cost");

            migrationBuilder.RenameColumn(
                name: "CostId",
                table: "DiscountVariation",
                newName: "PurchaseCostId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountVariation_CostId",
                table: "DiscountVariation",
                newName: "IX_DiscountVariation_PurchaseCostId");

            migrationBuilder.CreateTable(
                name: "PurchaseCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    InitialAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseCost_ItemUnit_Id",
                        column: x => x.Id,
                        principalTable: "ItemUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountVariation_PurchaseCost_PurchaseCostId",
                table: "DiscountVariation",
                column: "PurchaseCostId",
                principalTable: "PurchaseCost",
                principalColumn: "Id");
        }
    }
}
