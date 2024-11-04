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
            migrationBuilder.DropForeignKey(
                name: "FK_orders_promocodes_promo_code_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_promo_codes_promocodes_promo_code_id",
                table: "user_promo_codes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_promocodes",
                table: "promocodes");

            migrationBuilder.DropColumn(
                name: "confirm_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "event_medias");

            migrationBuilder.RenameTable(
                name: "promocodes",
                newName: "promo_codes");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_date",
                table: "promo_codes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "promo_codes",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_promo_codes",
                table: "promo_codes",
                column: "id");

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

            migrationBuilder.CreateIndex(
                name: "IX_promo_codes_unique_code",
                table: "promo_codes",
                column: "unique_code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_promo_codes_promo_code_id",
                table: "orders",
                column: "promo_code_id",
                principalTable: "promo_codes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_user_promo_codes_promo_codes_promo_code_id",
                table: "user_promo_codes",
                column: "promo_code_id",
                principalTable: "promo_codes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_promo_codes_promo_code_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_user_promo_codes_promo_codes_promo_code_id",
                table: "user_promo_codes");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_promo_codes",
                table: "promo_codes");

            migrationBuilder.DropIndex(
                name: "IX_promo_codes_unique_code",
                table: "promo_codes");

            migrationBuilder.DropColumn(
                name: "media_type",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "others",
                table: "medias");

            migrationBuilder.DropColumn(
                name: "deleted_date",
                table: "promo_codes");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "promo_codes");

            migrationBuilder.RenameTable(
                name: "promo_codes",
                newName: "promocodes");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_promocodes",
                table: "promocodes",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_promocodes_promo_code_id",
                table: "orders",
                column: "promo_code_id",
                principalTable: "promocodes",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_user_promo_codes_promocodes_promo_code_id",
                table: "user_promo_codes",
                column: "promo_code_id",
                principalTable: "promocodes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
