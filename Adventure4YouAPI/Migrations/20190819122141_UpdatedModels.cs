using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class UpdatedModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "UserLinks");

            migrationBuilder.AddColumn<int>(
                name: "PenaltyPerMinutLate",
                table: "Races",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PenaltyPerMinutLate",
                table: "Races");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "UserLinks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
