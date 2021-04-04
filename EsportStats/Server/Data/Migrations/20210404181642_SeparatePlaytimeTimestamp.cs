using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class SeparatePlaytimeTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PlaytimeTimestamp",
                table: "ExternalUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlaytimeTimestamp",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlaytimeTimestamp",
                table: "ExternalUsers");

            migrationBuilder.DropColumn(
                name: "PlaytimeTimestamp",
                table: "AspNetUsers");
        }
    }
}
