using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class MethodTestCase_AddHint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hint",
                schema: "domain",
                table: "MethodTestCases",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hint",
                schema: "domain",
                table: "MethodTestCases");
        }
    }
}
