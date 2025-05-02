using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Point.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Prices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayIndex",
                table: "PriceTypes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DisplayIndex",
                table: "PriceTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
