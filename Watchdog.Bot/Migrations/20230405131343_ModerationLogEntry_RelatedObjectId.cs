using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    /// <inheritdoc />
    public partial class ModerationLogEntry_RelatedObjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RelatedObjectId",
                table: "ModerationLog",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedObjectId",
                table: "ModerationLog");
        }
    }
}
