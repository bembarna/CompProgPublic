using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class test6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MethodTestCases",
                schema: "domain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignmentId = table.Column<int>(type: "int", nullable: false),
                    ParamInputs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Output = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PointValue = table.Column<int>(type: "int", nullable: false),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MethodTestInjectable = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MethodTestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MethodTestCases_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalSchema: "domain",
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MethodSignatures_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures",
                column: "MethodTestCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodTestCases_AssignmentId",
                schema: "domain",
                table: "MethodTestCases",
                column: "AssignmentId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodSignatures_MethodTestCases_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures");

            migrationBuilder.DropTable(
                name: "MethodTestCases",
                schema: "domain");

            migrationBuilder.DropIndex(
                name: "IX_MethodSignatures_MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures");

            migrationBuilder.DropColumn(
                name: "MethodTestCaseId",
                schema: "domain",
                table: "MethodSignatures");
        }
    }
}
