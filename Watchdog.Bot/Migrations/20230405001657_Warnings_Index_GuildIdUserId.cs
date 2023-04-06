using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    /// <inheritdoc />
    public partial class Warnings_Index_GuildIdUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warnings_GuildId",
                table: "Warnings");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_GuildId_UserId",
                table: "Warnings",
                columns: new[] { "GuildId", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Warnings_GuildId_UserId",
                table: "Warnings");

            migrationBuilder.CreateIndex(
                name: "IX_Warnings_GuildId",
                table: "Warnings",
                column: "GuildId");
        }
    }
}
