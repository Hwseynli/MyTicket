using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateOrderAndPromoCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_id",
                table: "tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "promocodes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unique_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_type = table.Column<int>(type: "integer", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    usage_limit = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedById = table.Column<int>(type: "integer", nullable: false),
                    RecordDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateById = table.Column<int>(type: "integer", nullable: true),
                    LastUpdateDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promocodes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_code = table.Column<string>(type: "text", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    promo_code_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_promocodes_promo_code_id",
                        column: x => x.promo_code_id,
                        principalTable: "promocodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_promo_codes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    promo_code_id = table.Column<int>(type: "integer", nullable: false),
                    used_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_promo_codes", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_promo_codes_promocodes_promo_code_id",
                        column: x => x.promo_code_id,
                        principalTable: "promocodes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_promo_codes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tickets_order_id",
                table: "tickets",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_promo_code_id",
                table: "orders",
                column: "promo_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_promo_codes_promo_code_id",
                table: "user_promo_codes",
                column: "promo_code_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_promo_codes_user_id",
                table: "user_promo_codes",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_orders_order_id",
                table: "tickets",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tickets_orders_order_id",
                table: "tickets");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "user_promo_codes");

            migrationBuilder.DropTable(
                name: "promocodes");

            migrationBuilder.DropIndex(
                name: "IX_tickets_order_id",
                table: "tickets");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "tickets");
        }
    }
}
