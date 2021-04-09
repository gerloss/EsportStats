using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class AddMatchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MatchId",
                table: "TopListEntries",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "TopListEntries");
        }
    }
}
