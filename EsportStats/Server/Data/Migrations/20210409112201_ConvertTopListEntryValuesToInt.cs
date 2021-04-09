using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class ConvertTopListEntryValuesToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "TopListEntries",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "TopListEntries",
                type: "float",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
