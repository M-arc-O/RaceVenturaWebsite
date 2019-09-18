using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class AddedTeamNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Teams");
        }
    }
}
