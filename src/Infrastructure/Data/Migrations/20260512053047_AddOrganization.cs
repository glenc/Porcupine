using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Porcupine.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    MarketSegmentId = table.Column<int>(type: "INTEGER", nullable: true),
                    IndustryId = table.Column<int>(type: "INTEGER", nullable: true),
                    LifecycleStageId = table.Column<int>(type: "INTEGER", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    LastModified = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Organizations_LifecycleStages_LifecycleStageId",
                        column: x => x.LifecycleStageId,
                        principalTable: "LifecycleStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizations_MarketSegments_MarketSegmentId",
                        column: x => x.MarketSegmentId,
                        principalTable: "MarketSegments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_IndustryId",
                table: "Organizations",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_LifecycleStageId",
                table: "Organizations",
                column: "LifecycleStageId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_MarketSegmentId",
                table: "Organizations",
                column: "MarketSegmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}
