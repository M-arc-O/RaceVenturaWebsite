using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class Expanded_Point_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "Points",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Points",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Points");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "Points");
        }
    }
}
