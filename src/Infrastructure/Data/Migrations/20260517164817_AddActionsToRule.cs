using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Porcupine.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActionsToRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Actions",
                table: "Rules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actions",
                table: "Rules");
        }
    }
}
