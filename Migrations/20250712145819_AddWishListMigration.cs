using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rememorize.Migrations
{
    /// <inheritdoc />
    public partial class AddWishListMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Books_BookId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_BookId",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "WishLists");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "WishLists",
                newName: "Quantity");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "WishLists",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookName",
                table: "WishLists",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "BookName",
                table: "WishLists");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "WishLists",
                newName: "BookId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "WishLists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_BookId",
                table: "WishLists",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Books_BookId",
                table: "WishLists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
