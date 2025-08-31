using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addpopUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "dbo",
                table: "UserDiet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PopUps",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopUps", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiet_ParentId",
                schema: "dbo",
                table: "UserDiet",
                column: "ParentId");

      

          

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userAnswers_UserDiet_UserDietId",
                schema: "dbo",
                table: "userAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDiet_UserDiet_ParentId",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropTable(
                name: "PopUps",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_UserDiet_ParentId",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropIndex(
                name: "IX_userAnswers_UserDietId",
                schema: "dbo",
                table: "userAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "ParentId",
                schema: "dbo",
                table: "UserDiet",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
