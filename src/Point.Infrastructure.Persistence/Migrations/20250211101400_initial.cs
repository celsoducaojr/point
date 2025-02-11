using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JobOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOrder", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ArticleCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArticleUnitId = table.Column<int>(type: "int", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    WholesalePrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PriceCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArticleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyTag_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceReference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InitialPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceReference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceReference_Price_Id",
                        column: x => x.Id,
                        principalTable: "Price",
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
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
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
                name: "IX_ArticleTag_ArticleId",
                table: "ArticleTag",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyTag_CompanyId",
                table: "CompanyTag",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_ArticleId",
                table: "Price",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceVariation_PriceReferenceId",
                table: "PriceVariation",
                column: "PriceReferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropTable(
                name: "CompanyTag");

            migrationBuilder.DropTable(
                name: "JobOrder");

            migrationBuilder.DropTable(
                name: "PriceVariation");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "PriceReference");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "Article");
        }
    }
}
