using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class parentiduserdiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnother",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                schema: "dbo",
                table: "UserDiet",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnother",
                schema: "dbo",
                table: "UserDiet",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
