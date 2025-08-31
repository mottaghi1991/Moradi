using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addformtype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstForm",
                schema: "dbo",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserDiet",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DietId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDiet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDiet_Diets_DietId",
                        column: x => x.DietId,
                        principalSchema: "dbo",
                        principalTable: "Diets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDiet_MyUser_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "MyUser",
                        principalColumn: "ItUserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDiet_DietId",
                schema: "dbo",
                table: "UserDiet",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDiet_UserId",
                schema: "dbo",
                table: "UserDiet",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDiet",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "FirstForm",
                schema: "dbo",
                table: "Questions");
        }
    }
}
