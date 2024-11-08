using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubCategoryAndEventTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_sub_categories_sub_category_id",
                table: "events");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_categories_categories_category_id",
                table: "sub_categories");

            migrationBuilder.DropIndex(
                name: "IX_sub_categories_category_id",
                table: "sub_categories");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "sub_categories");

            migrationBuilder.RenameColumn(
                name: "sub_category_id",
                table: "events",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_events_sub_category_id",
                table: "events",
                newName: "IX_events_CategoryId");

            migrationBuilder.CreateTable(
                name: "category_subcategories",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_subcategories", x => new { x.CategoriesId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_category_subcategories_categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_subcategories_sub_categories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "sub_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_subcategories",
                columns: table => new
                {
                    EventsId = table.Column<int>(type: "integer", nullable: false),
                    SubCategoriesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_subcategories", x => new { x.EventsId, x.SubCategoriesId });
                    table.ForeignKey(
                        name: "FK_event_subcategories_events_EventsId",
                        column: x => x.EventsId,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_event_subcategories_sub_categories_SubCategoriesId",
                        column: x => x.SubCategoriesId,
                        principalTable: "sub_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_subcategories_SubCategoriesId",
                table: "category_subcategories",
                column: "SubCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_event_subcategories_SubCategoriesId",
                table: "event_subcategories",
                column: "SubCategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_events_categories_CategoryId",
                table: "events",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_categories_CategoryId",
                table: "events");

            migrationBuilder.DropTable(
                name: "category_subcategories");

            migrationBuilder.DropTable(
                name: "event_subcategories");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "events",
                newName: "sub_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_events_CategoryId",
                table: "events",
                newName: "IX_events_sub_category_id");

            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "sub_categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_category_id",
                table: "sub_categories",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_events_sub_categories_sub_category_id",
                table: "events",
                column: "sub_category_id",
                principalTable: "sub_categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_categories_categories_category_id",
                table: "sub_categories",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
