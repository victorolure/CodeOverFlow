using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeOverFlow.Data.Migrations
{
    public partial class addCommentsToQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_QuestionId",
                table: "Comment",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Question_QuestionId",
                table: "Comment",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Question_QuestionId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_QuestionId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Comment");
        }
    }
}
