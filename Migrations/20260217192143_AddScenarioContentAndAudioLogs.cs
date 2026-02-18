using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheSignal.Migrations
{
    /// <inheritdoc />
    public partial class AddScenarioContentAndAudioLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentKey",
                table: "Locations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsClassExclusive",
                table: "Locations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequiredClass",
                table: "Locations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredClass",
                table: "LocationExits",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredCombat",
                table: "LocationExits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredScience",
                table: "LocationExits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredTechnical",
                table: "LocationExits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Effect",
                table: "Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStoryBeat",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartingLocationId",
                table: "Characters",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AudioLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogId = table.Column<string>(type: "TEXT", nullable: false),
                    Day = table.Column<int>(type: "INTEGER", nullable: false),
                    Speaker = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Transcript = table.Column<string>(type: "TEXT", nullable: false),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsClassSpecific = table.Column<bool>(type: "INTEGER", nullable: false),
                    RequiredClass = table.Column<string>(type: "TEXT", nullable: true),
                    CharacterId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioLogs_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioLogs_CharacterId",
                table: "AudioLogs",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioLogs");

            migrationBuilder.DropColumn(
                name: "ContentKey",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsClassExclusive",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RequiredClass",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RequiredClass",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "RequiredCombat",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "RequiredScience",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "RequiredTechnical",
                table: "LocationExits");

            migrationBuilder.DropColumn(
                name: "Effect",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CurrentStoryBeat",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "StartingLocationId",
                table: "Characters");
        }
    }
}
