using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class edituseranswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_userAnswers_UserDietId",
                schema: "dbo",
                table: "userAnswers",
                column: "UserDietId");

            migrationBuilder.AddForeignKey(
                name: "FK_userAnswers_UserDiet_UserDietId",
                schema: "dbo",
                table: "userAnswers",
                column: "UserDietId",
                principalSchema: "dbo",
                principalTable: "UserDiet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
