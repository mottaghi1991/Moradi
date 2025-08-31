using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class adduserfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileList",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserDietId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFile = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileList_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileList_UserDiet_UserDietId",
                        column: x => x.UserDietId,
                        principalSchema: "dbo",
                        principalTable: "UserDiet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileList_QuestionId",
                schema: "dbo",
                table: "FileList",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FileList_UserDietId",
                schema: "dbo",
                table: "FileList",
                column: "UserDietId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileList",
                schema: "dbo");
        }
    }
}
