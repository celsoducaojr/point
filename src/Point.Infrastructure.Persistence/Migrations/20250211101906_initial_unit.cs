using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial_unit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Unit",
                table: "Unit");

            migrationBuilder.RenameTable(
                name: "Unit",
                newName: "ArticleUnit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleUnit",
                table: "ArticleUnit",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleUnit",
                table: "ArticleUnit");

            migrationBuilder.RenameTable(
                name: "ArticleUnit",
                newName: "Unit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Unit",
                table: "Unit",
                column: "Id");
        }
    }
}
