using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Watchdog.Bot.Migrations
{
    /// <inheritdoc />
    public partial class Warning_FixedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "Warnings",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "current_timestamp");

            migrationBuilder.AddForeignKey(
                name: "FK_Warnings_Guilds_GuildId",
                table: "Warnings",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warnings_Guilds_GuildId",
                table: "Warnings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Warnings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "current_timestamp",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }
    }
}
