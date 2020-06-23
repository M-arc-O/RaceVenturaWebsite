using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class AddedTeamCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Teams");
        }
    }
}
