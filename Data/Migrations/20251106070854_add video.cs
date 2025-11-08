using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addvideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.AddColumn<string>(
                name: "Video",
                schema: "dbo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Video",
                schema: "dbo",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                schema: "dbo",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "dbo",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                schema: "dbo",
                table: "Products",
                column: "CategoryId",
                principalSchema: "dbo",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
