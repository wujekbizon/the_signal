using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSignal.Migrations
{
    /// <inheritdoc />
    public partial class AddAIDungeonMasterSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Deck",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsStartingLocation",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStatic",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Locations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GapMaxSteps",
                table: "LocationExits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GapMinSteps",
                table: "LocationExits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GapSize",
                table: "LocationExits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GapType",
                table: "LocationExits",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProceduralRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    AudioDescription = table.Column<string>(type: "TEXT", nullable: false),
                    EnvironmentalDescription = table.Column<string>(type: "TEXT", nullable: false),
                    TwistLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    GapSequenceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    GapType = table.Column<string>(type: "TEXT", nullable: false),
                    HealthChange = table.Column<int>(type: "INTEGER", nullable: false),
                    SanityChange = table.Column<int>(type: "INTEGER", nullable: false),
                    OxygenChange = table.Column<int>(type: "INTEGER", nullable: false),
                    FoundItemName = table.Column<string>(type: "TEXT", nullable: true),
                    DiscoveryClue = table.Column<string>(type: "TEXT", nullable: true),
                    SourceAnchorId = table.Column<int>(type: "INTEGER", nullable: true),
                    TargetAnchorId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsBacktrack = table.Column<bool>(type: "INTEGER", nullable: false),
                    VisitCount = table.Column<int>(type: "INTEGER", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastVisitedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ContextHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProceduralRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProceduralRooms_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: false),
                    StepsFromLastAnchor = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentTwistLevel = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetStaticLocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousRoomId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsInProceduralGap = table.Column<bool>(type: "INTEGER", nullable: false),
                    RemainingGapSteps = table.Column<int>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationStates_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExplorationStates_ProceduralRooms_PreviousRoomId",
                        column: x => x.PreviousRoomId,
                        principalTable: "ProceduralRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationStates_CharacterId",
                table: "ExplorationStates",
                column: "CharacterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationStates_PreviousRoomId",
                table: "ExplorationStates",
                column: "PreviousRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_ProceduralRooms_CharacterId_ContextHash",
                table: "ProceduralRooms",
                columns: new[] { "CharacterId", "ContextHash" });

            migrationBuilder.CreateIndex(
                name: "IX_ProceduralRooms_LastVisitedAt",
                table: "ProceduralRooms",
                column: "LastVisitedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplorationStates");

            migrationBuilder.DropTable(
                name: "ProceduralRooms");

            migrationBuilder.DropColumn(
                name: "Deck",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsStartingLocation",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsStatic",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "GapMaxSteps",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "GapMinSteps",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "GapSize",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "GapType",
                table: "LocationExits");
        }
    }
}
