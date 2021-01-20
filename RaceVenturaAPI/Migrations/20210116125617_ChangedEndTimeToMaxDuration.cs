using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RaceVenturaAPI.Migrations
{
    public partial class ChangedEndTimeToMaxDuration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Races");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxDuration",
                table: "Races",
                type: "interval",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxDuration",
                table: "Races");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Races",
                type: "timestamp without time zone",
                nullable: true);
        }
    }
}
