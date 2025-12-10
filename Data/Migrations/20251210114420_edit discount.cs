using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editdiscount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_MyUser_UserId",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_UserId",
                schema: "dbo",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "dbo",
                table: "Discounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "dbo",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_UserId",
                schema: "dbo",
                table: "Discounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_MyUser_UserId",
                schema: "dbo",
                table: "Discounts",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "MyUser",
                principalColumn: "ItUserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
