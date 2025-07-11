using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rememorize.Migrations
{
    /// <inheritdoc />
    public partial class NewModelChangedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyCount",
                table: "BuyHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyCount",
                table: "BuyHistories");
        }
    }
}
