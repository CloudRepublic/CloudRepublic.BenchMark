using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudRepublic.BenchMark.Data.Migrations
{
    public partial class AddedRunPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CallPosition",
                table: "BenchMarkResult",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallPosition",
                table: "BenchMarkResult");
        }
    }
}
