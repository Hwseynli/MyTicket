using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndPromoCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirm_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "event_medias");

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "promocodes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "promocodes",
                type: "boolean",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "media_type",
                table: "medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "others",
                table: "medias",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "promocodes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "promocodes");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "others",
                table: "medias");

            migrationBuilder.AddColumn<string>(
                name: "confirm_token",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "media_type",
                table: "event_medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
