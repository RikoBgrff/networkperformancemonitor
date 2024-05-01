using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace networkperformancemonitor.Migrations
{
    public partial class newpush : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UrlToIp",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "UrlToIp",
                table: "TestResults");
        }
    }
}
