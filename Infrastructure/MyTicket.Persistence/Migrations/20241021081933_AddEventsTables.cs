using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEventsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "seat_count",
                table: "place_halls",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    start_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    language = table.Column<string>(type: "text", nullable: false),
                    min_age = table.Column<byte>(type: "smallint", nullable: false),
                    sub_category_id = table.Column<int>(type: "integer", nullable: false),
                    place_hall_id = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    average_rating = table.Column<double>(type: "double precision", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_events_place_halls_place_hall_id",
                        column: x => x.place_hall_id,
                        principalTable: "place_halls",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_events_sub_categories_sub_category_id",
                        column: x => x.sub_category_id,
                        principalTable: "sub_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_medias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    media_type = table.Column<int>(type: "integer", nullable: false),
                    event_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_medias", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_medias_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_ratings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    rating_value = table.Column<int>(type: "integer", nullable: false),
                    rated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_ratings", x => x.id);
                    table.ForeignKey(
                        name: "FK_event_ratings_events_EventId",
                        column: x => x.EventId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_event_ratings_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    event_media_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<int>(type: "integer", nullable: false),
                    record_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    update_by_id = table.Column<int>(type: "integer", nullable: true),
                    last_update_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medias", x => x.id);
                    table.ForeignKey(
                        name: "FK_medias_event_medias_event_media_id",
                        column: x => x.event_media_id,
                        principalTable: "event_medias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_medias_event_id",
                table: "event_medias",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_ratings_EventId",
                table: "event_ratings",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_event_ratings_UserId",
                table: "event_ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_events_place_hall_id",
                table: "events",
                column: "place_hall_id");

            migrationBuilder.CreateIndex(
                name: "IX_events_sub_category_id",
                table: "events",
                column: "sub_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_medias_event_media_id",
                table: "medias",
                column: "event_media_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_ratings");

            migrationBuilder.DropTable(
                name: "medias");

            migrationBuilder.DropTable(
                name: "event_medias");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropColumn(
                name: "seat_count",
                table: "place_halls");
        }
    }
}
