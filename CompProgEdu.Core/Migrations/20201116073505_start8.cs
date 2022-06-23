using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class start8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodSignatures_MethodTestCases_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodSignatures_MethodTestCases_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures",
                column: "MethodTestCaseId",
                principalSchema: "domain",
                principalTable: "MethodTestCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodSignatures_MethodTestCases_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodSignatures_MethodTestCases_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures",
                column: "MethodTestCaseId",
                principalSchema: "domain",
                principalTable: "MethodTestCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
