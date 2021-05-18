using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudRepublic.BenchMark.Data.Migrations
{
    public partial class Adding_FunctionVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FunctionVersion",
                table: "BenchMarkResult",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctionVersion",
                table: "BenchMarkResult");
        }
    }
}
