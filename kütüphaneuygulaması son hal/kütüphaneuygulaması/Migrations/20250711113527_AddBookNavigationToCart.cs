using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kütüphaneuygulaması.Migrations
{
    /// <inheritdoc />
    public partial class AddBookNavigationToCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cart_BookId",
                table: "Cart",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Book_BookId",
                table: "Cart",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Book_BookId",
                table: "Cart");

            migrationBuilder.DropIndex(
                name: "IX_Cart_BookId",
                table: "Cart");
        }
    }
}
