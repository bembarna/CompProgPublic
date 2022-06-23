using Microsoft.EntityFrameworkCore.Migrations;

namespace CompProgEdu.Core.Migrations
{
    public partial class Start4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertySignatures_CurlySets_CurlySetId",
                schema: "domain",
                table: "PropertySignatures");

            migrationBuilder.AlterColumn<int>(
                name: "CurlySetId",
                schema: "domain",
                table: "PropertySignatures",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySignatures_CurlySets_CurlySetId",
                schema: "domain",
                table: "PropertySignatures",
                column: "CurlySetId",
                principalSchema: "domain",
                principalTable: "CurlySets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertySignatures_CurlySets_CurlySetId",
                schema: "domain",
                table: "PropertySignatures");

            migrationBuilder.AlterColumn<int>(
                name: "CurlySetId",
                schema: "domain",
                table: "PropertySignatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertySignatures_CurlySets_CurlySetId",
                schema: "domain",
                table: "PropertySignatures",
                column: "CurlySetId",
                principalSchema: "domain",
                principalTable: "CurlySets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
