using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class AddUserHoursPlayed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HoursPlayed",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursPlayed",
                table: "AspNetUsers");
        }
    }
}
