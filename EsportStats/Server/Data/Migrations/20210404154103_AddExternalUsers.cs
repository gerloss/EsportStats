using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EsportStats.Server.Data.Migrations
{
    public partial class AddExternalUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopListEntries_AspNetUsers_UserId",
                table: "TopListEntries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TopListEntries",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<decimal>(
                name: "ExternalUserId",
                table: "TopListEntries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalUsers",
                columns: table => new
                {
                    SteamId = table.Column<decimal>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ProfileUrl = table.Column<string>(nullable: false),
                    Avatar = table.Column<string>(nullable: false),
                    AvatarFull = table.Column<string>(nullable: false),
                    Playtime = table.Column<int>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalUsers", x => x.SteamId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopListEntries_ExternalUserId",
                table: "TopListEntries",
                column: "ExternalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopListEntries_ExternalUsers_ExternalUserId",
                table: "TopListEntries",
                column: "ExternalUserId",
                principalTable: "ExternalUsers",
                principalColumn: "SteamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopListEntries_AspNetUsers_UserId",
                table: "TopListEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopListEntries_ExternalUsers_ExternalUserId",
                table: "TopListEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TopListEntries_AspNetUsers_UserId",
                table: "TopListEntries");

            migrationBuilder.DropTable(
                name: "ExternalUsers");

            migrationBuilder.DropIndex(
                name: "IX_TopListEntries_ExternalUserId",
                table: "TopListEntries");

            migrationBuilder.DropColumn(
                name: "ExternalUserId",
                table: "TopListEntries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TopListEntries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TopListEntries_AspNetUsers_UserId",
                table: "TopListEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
