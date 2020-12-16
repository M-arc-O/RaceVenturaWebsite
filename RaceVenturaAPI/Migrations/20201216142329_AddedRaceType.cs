using Microsoft.EntityFrameworkCore.Migrations;

namespace RaceVenturaAPI.Migrations
{
    public partial class AddedRaceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceType",
                table: "Races",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaceType",
                table: "Races");
        }
    }
}
