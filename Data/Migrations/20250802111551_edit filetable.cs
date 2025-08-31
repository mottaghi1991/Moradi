using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editfiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileList_Questions_QuestionId",
                schema: "dbo",
                table: "FileList");

            migrationBuilder.DropIndex(
                name: "IX_FileList_QuestionId",
                schema: "dbo",
                table: "FileList");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                schema: "dbo",
                table: "FileList");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                schema: "dbo",
                table: "FileList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FileList_QuestionId",
                schema: "dbo",
                table: "FileList",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileList_Questions_QuestionId",
                schema: "dbo",
                table: "FileList",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
