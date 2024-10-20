using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyTicket.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSubscribersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_subscribers_email",
                table: "subscribers");

            migrationBuilder.DropIndex(
                name: "IX_subscribers_phone_number",
                table: "subscribers");

            migrationBuilder.DropColumn(
                name: "email",
                table: "subscribers");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "subscribers");

            migrationBuilder.AddColumn<string>(
                name: "email_or_phone_number",
                table: "subscribers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "string_type",
                table: "subscribers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_email_or_phone_number",
                table: "subscribers",
                column: "email_or_phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_subscribers_email_or_phone_number",
                table: "subscribers");

            migrationBuilder.DropColumn(
                name: "email_or_phone_number",
                table: "subscribers");

            migrationBuilder.DropColumn(
                name: "string_type",
                table: "subscribers");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "subscribers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "subscribers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_email",
                table: "subscribers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_phone_number",
                table: "subscribers",
                column: "phone_number",
                unique: true);
        }
    }
}
