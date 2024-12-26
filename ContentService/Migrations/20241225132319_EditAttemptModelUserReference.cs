using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContentService.Migrations
{
    /// <inheritdoc />
    public partial class EditAttemptModelUserReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Attempts");

            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Attempts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Attempts");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Attempts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
