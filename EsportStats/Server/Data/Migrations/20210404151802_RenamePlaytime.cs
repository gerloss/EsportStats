using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class RenamePlaytime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursPlayed",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Playtime",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Playtime",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "HoursPlayed",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
