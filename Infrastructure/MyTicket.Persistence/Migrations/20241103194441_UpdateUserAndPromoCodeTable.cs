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
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "is_main",
                table: "medias",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

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

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_name",
                table: "sub_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_settings_key",
                table: "settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roles_role_name",
                table: "roles",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_promocodes_unique_code",
                table: "promocodes",
                column: "unique_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_places_name",
                table: "places",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_place_halls_name",
                table: "place_halls",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_order_code",
                table: "orders",
                column: "order_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_events_title",
                table: "events",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_name",
                table: "categories",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_sub_categories_name",
                table: "sub_categories");

            migrationBuilder.DropIndex(
                name: "IX_settings_key",
                table: "settings");

            migrationBuilder.DropIndex(
                name: "IX_roles_role_name",
                table: "roles");

            migrationBuilder.DropIndex(
                name: "IX_promocodes_unique_code",
                table: "promocodes");

            migrationBuilder.DropIndex(
                name: "IX_places_name",
                table: "places");

            migrationBuilder.DropIndex(
                name: "IX_place_halls_name",
                table: "place_halls");

            migrationBuilder.DropIndex(
                name: "IX_orders_order_code",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_events_title",
                table: "events");

            migrationBuilder.DropIndex(
                name: "IX_categories_name",
                table: "categories");

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

            migrationBuilder.AlterColumn<bool>(
                name: "is_main",
                table: "medias",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "media_type",
                table: "event_medias",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
