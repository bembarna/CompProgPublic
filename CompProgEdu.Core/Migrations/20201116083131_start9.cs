using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class start9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturnType",
                schema: "domain",
                table: "MethodTestCases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                schema: "domain",
                table: "CurlySets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnType",
                schema: "domain",
                table: "MethodTestCases");

            migrationBuilder.DropColumn(
                name: "IsMain",
                schema: "domain",
                table: "CurlySets");
        }
    }
}
