using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addposttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_MyUser_myUserItUserId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_myUserItUserId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "myUserItUserId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Questions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Diets",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Diets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                schema: "dbo",
                table: "Comments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_MyUser_UserId",
                schema: "dbo",
                table: "Comments",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "MyUser",
                principalColumn: "ItUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_MyUser_UserId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                schema: "dbo",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Diets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                schema: "dbo",
                table: "Diets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "myUserItUserId",
                schema: "dbo",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_myUserItUserId",
                schema: "dbo",
                table: "Comments",
                column: "myUserItUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_MyUser_myUserItUserId",
                schema: "dbo",
                table: "Comments",
                column: "myUserItUserId",
                principalSchema: "dbo",
                principalTable: "MyUser",
                principalColumn: "ItUserId");
        }
    }
}
