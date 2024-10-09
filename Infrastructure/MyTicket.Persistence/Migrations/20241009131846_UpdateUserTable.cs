using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "mail",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "users",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "users",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "users",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "activated",
                table: "users",
                newName: "is_activated");

            migrationBuilder.RenameIndex(
                name: "IX_users_mail",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_time",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_time",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "users",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "users",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "is_activated",
                table: "users",
                newName: "activated");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "users",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "mail");

            migrationBuilder.RenameIndex(
                name: "IX_users_email",
                table: "users",
                newName: "IX_users_mail");
        }
    }
}
