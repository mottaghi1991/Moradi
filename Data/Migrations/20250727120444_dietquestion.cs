using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class dietquestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DietQuestions",
                schema: "dbo",
                columns: table => new
                {
                    DietsId = table.Column<int>(type: "int", nullable: false),
                    QuestionsId = table.Column<int>(type: "int", nullable: false),
                    DietId = table.Column<int>(type: "int", nullable: true),
                    QuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietQuestions", x => new { x.DietsId, x.QuestionsId });
                    table.ForeignKey(
                        name: "FK_DietQuestions_Diets_DietId",
                        column: x => x.DietId,
                        principalSchema: "dbo",
                        principalTable: "Diets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DietQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "Questions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DietQuestions_DietId",
                schema: "dbo",
                table: "DietQuestions",
                column: "DietId");

            migrationBuilder.CreateIndex(
                name: "IX_DietQuestions_QuestionId",
                schema: "dbo",
                table: "DietQuestions",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietQuestions",
                schema: "dbo");
        }
    }
}
