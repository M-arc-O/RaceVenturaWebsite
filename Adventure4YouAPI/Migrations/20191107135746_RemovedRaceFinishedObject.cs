using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class RemovedRaceFinishedObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamPhones");

            migrationBuilder.DropTable(
                name: "TeamRacesFinished");

            migrationBuilder.AddColumn<DateTime>(
                name: "RaceFinished",
                table: "Teams",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaceFinished",
                table: "Teams");

            migrationBuilder.CreateTable(
                name: "TeamPhones",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PhoneId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPhones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPhones_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamRacesFinished",
                columns: table => new
                {
                    TeamRaceFinishedId = table.Column<Guid>(nullable: false),
                    RaceId = table.Column<Guid>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRacesFinished", x => x.TeamRaceFinishedId);
                    table.ForeignKey(
                        name: "FK_TeamRacesFinished_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamPhones_TeamId",
                table: "TeamPhones",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamRacesFinished_TeamId",
                table: "TeamRacesFinished",
                column: "TeamId",
                unique: true);
        }
    }
}
