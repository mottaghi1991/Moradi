using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class useransweredit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDietId",
                schema: "dbo",
                table: "userAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_userAnswers_UserDietId",
                schema: "dbo",
                table: "userAnswers",
                column: "UserDietId");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userAnswers_UserDiet_UserDietId",
                schema: "dbo",
                table: "userAnswers");

            migrationBuilder.DropIndex(
                name: "IX_userAnswers_UserDietId",
                schema: "dbo",
                table: "userAnswers");

            migrationBuilder.DropColumn(
                name: "UserDietId",
                schema: "dbo",
                table: "userAnswers");
        }
    }
}
