using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Adventure4YouAPI.Migrations
{
    public partial class UpdateForPointRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActiveStage",
                table: "Teams",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AllowedCoordinatesDeviation",
                table: "Races",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "RegisteredId",
                columns: table => new
                {
                    RegisteredIdId = table.Column<Guid>(nullable: false),
                    TeamId = table.Column<Guid>(nullable: false),
                    UniqueId = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredId", x => x.RegisteredIdId);
                    table.ForeignKey(
                        name: "FK_RegisteredId_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredId_TeamId",
                table: "RegisteredId",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegisteredId");

            migrationBuilder.DropColumn(
                name: "ActiveStage",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AllowedCoordinatesDeviation",
                table: "Races");
        }
    }
}
