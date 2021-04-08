using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class AddHeroStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HeroStatsTimestamp",
                table: "ExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HeroStatsTimestamp",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "HeroStats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SteamId = table.Column<decimal>(nullable: false),
                    Hero = table.Column<int>(nullable: false),
                    Games = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeroStats", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeroStats");

            migrationBuilder.DropColumn(
                name: "HeroStatsTimestamp",
                table: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "HeroStatsTimestamp",
                table: "AspNetUsers");

        }
    }
}
