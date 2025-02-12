using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceVariation");

            migrationBuilder.DropTable(
                name: "PriceReference");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Supplier",
                newName: "Remarks");

            migrationBuilder.CreateTable(
                name: "PurchaseCost",
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
                    table.PrimaryKey("PK_PurchaseCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseCost_ItemUnit_Id",
                        column: x => x.Id,
                        principalTable: "ItemUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CostVariation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchaseCostId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostVariation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostVariation_PurchaseCost_PurchaseCostId",
                        column: x => x.PurchaseCostId,
                        principalTable: "PurchaseCost",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CostVariation_PurchaseCostId",
                table: "CostVariation",
                column: "PurchaseCostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostVariation");

            migrationBuilder.DropTable(
                name: "PurchaseCost");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Supplier",
                newName: "Notes");

            migrationBuilder.CreateTable(
                name: "PriceReference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    InitialPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceReference_ItemUnit_Id",
                        column: x => x.Id,
                        principalTable: "ItemUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceVariation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PriceReferenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceVariation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceVariation_PriceReference_PriceReferenceId",
                        column: x => x.PriceReferenceId,
                        principalTable: "PriceReference",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PriceVariation_PriceReferenceId",
                table: "PriceVariation",
                column: "PriceReferenceId");
        }
    }
}
