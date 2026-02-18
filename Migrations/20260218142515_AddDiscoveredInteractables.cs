using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSignal.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscoveredInteractables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscoveredInteractables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    InteractableId = table.Column<string>(type: "TEXT", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiscoveredAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    HasInteracted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveredInteractables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscoveredInteractables_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveredInteractables_CharacterId_InteractableId",
                table: "DiscoveredInteractables",
                columns: new[] { "CharacterId", "InteractableId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveredInteractables_LocationId",
                table: "DiscoveredInteractables",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscoveredInteractables");
        }
    }
}
