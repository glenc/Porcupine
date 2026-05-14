using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Porcupine.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RulesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventName",
                table: "Rules");

            migrationBuilder.AddColumn<string>(
                name: "TriggerName",
                table: "Rules",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TriggerType",
                table: "Rules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerName",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "Rules");

            migrationBuilder.AddColumn<string>(
                name: "EventName",
                table: "Rules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
