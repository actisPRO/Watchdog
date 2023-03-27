using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    /// <inheritdoc />
    public partial class PermissionsRemoved_LogsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionOverrides");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.CreateTable(
                name: "ModerationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GuildId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    ExecutorId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    TargetId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModerationLog_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModerationLog_GuildId",
                table: "ModerationLog",
                column: "GuildId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModerationLog");

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
    }
}
