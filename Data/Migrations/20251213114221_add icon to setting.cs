using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addicontosetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconFirst",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconFirstLink",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconSecond",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconSecondLink",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconThird",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconThirdLink",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconFirst",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IconFirstLink",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IconSecond",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IconSecondLink",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IconThird",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "IconThirdLink",
                schema: "dbo",
                table: "Settings");
        }
    }
}
