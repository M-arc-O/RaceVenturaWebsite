using Microsoft.EntityFrameworkCore.Migrations;

namespace RaceVenturaAPI.Migrations
{
    public partial class WithPointInformationText : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PointInformationText",
                table: "Races",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointInformationText",
                table: "Races");
        }
    }
}
