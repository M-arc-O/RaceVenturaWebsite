using Microsoft.EntityFrameworkCore.Migrations;

namespace RaceVenturaAPI.Migrations
{
    public partial class AddedRaceAccessLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RaceAccess",
                table: "UserLinks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaceAccess",
                table: "UserLinks");
        }
    }
}
