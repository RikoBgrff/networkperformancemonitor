using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace networkperformancemonitor.Migrations
{
    public partial class newpush2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestType",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestType",
                table: "TestResults");
        }
    }
}
