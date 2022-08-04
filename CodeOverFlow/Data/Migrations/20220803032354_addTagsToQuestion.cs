using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeOverFlow.Data.Migrations
{
    public partial class addTagsToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Tags",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_QuestionId",
                table: "Tags",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Question_QuestionId",
                table: "Tags",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Question_QuestionId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_QuestionId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Tags");
        }
    }
}
