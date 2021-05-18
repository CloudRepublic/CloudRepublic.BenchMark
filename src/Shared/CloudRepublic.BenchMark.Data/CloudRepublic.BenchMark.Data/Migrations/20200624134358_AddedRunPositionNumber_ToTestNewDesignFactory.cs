using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudRepublic.BenchMark.Data.Migrations
{
    public partial class AddedRunPositionNumber_ToTestNewDesignFactory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallPosition",
                table: "BenchMarkResult");

            migrationBuilder.AddColumn<int>(
                name: "CallPositionNumber",
                table: "BenchMarkResult",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallPositionNumber",
                table: "BenchMarkResult");

            migrationBuilder.AddColumn<int>(
                name: "CallPosition",
                table: "BenchMarkResult",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
