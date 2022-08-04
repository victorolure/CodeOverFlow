using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeOverFlow.Data.Migrations
{
    public partial class addAnswerToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AnswerId",
                table: "Comment",
                column: "AnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Answer_AnswerId",
                table: "Comment",
                column: "AnswerId",
                principalTable: "Answer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Answer_AnswerId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AnswerId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AnswerId",
                table: "Comment");
        }
    }
}
