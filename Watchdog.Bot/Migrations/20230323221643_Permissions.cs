using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    /// <inheritdoc />
    public partial class Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    RestrictedAction = table.Column<string>(type: "text", nullable: false),
                    RequiredPermission = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.RestrictedAction);
                });

            migrationBuilder.CreateTable(
                name: "PermissionOverrides",
                columns: table => new
                {
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    RestrictedAction = table.Column<string>(type: "text", nullable: false),
                    Override = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionOverrides", x => new { x.GuildId, x.RestrictedAction });
                    table.ForeignKey(
                        name: "FK_PermissionOverrides_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermissionOverrides_Permissions_RestrictedAction",
                        column: x => x.RestrictedAction,
                        principalTable: "Permissions",
                        principalColumn: "RestrictedAction",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionOverrides_RestrictedAction",
                table: "PermissionOverrides",
                column: "RestrictedAction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionOverrides");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
