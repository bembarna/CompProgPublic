using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class Start3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MethodName",
                schema: "domain",
                table: "MethodSignatures",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MethodName",
                schema: "domain",
                table: "MethodSignatures");
        }
    }
}
