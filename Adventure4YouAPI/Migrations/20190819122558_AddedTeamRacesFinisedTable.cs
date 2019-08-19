using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class AddedTeamRacesFinisedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamRacesFinished",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RaceId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamRacesFinished", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamRacesFinished");
        }
    }
}
