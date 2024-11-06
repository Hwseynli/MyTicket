using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndWishListTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wish_lists_users_user_id",
                table: "wish_lists");

            migrationBuilder.DropIndex(
                name: "IX_wish_lists_user_id",
                table: "wish_lists");

            migrationBuilder.AddColumn<int>(
                name: "wish_list_id",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_wish_list_id",
                table: "users",
                column: "wish_list_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_wish_lists_wish_list_id",
                table: "users",
                column: "wish_list_id",
                principalTable: "wish_lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_wish_lists_wish_list_id",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_wish_list_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "wish_list_id",
                table: "users");

            migrationBuilder.CreateIndex(
                name: "IX_wish_lists_user_id",
                table: "wish_lists",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_wish_lists_users_user_id",
                table: "wish_lists",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
