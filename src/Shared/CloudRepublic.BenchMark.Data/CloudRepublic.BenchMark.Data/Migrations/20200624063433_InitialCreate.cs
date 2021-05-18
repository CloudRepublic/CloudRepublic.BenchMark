using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CloudRepublic.BenchMark.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenchMarkResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CloudProvider = table.Column<int>(nullable: false),
                    HostingEnvironment = table.Column<int>(nullable: false),
                    Runtime = table.Column<int>(nullable: false),
                    Success = table.Column<bool>(nullable: false),
                    RequestDuration = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false, defaultValueSql: "(getdate())"),
                    IsColdRequest = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BenchMarkResults_pk", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "BenchMarkResults__CreatedAt_index",
                table: "BenchMarkResult",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "BenchMarkResults_Id_uindex",
                table: "BenchMarkResult",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchMarkResult");
        }
    }
}
