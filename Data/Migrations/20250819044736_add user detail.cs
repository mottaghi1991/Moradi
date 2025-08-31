using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class adduserdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "dbo",
                table: "MyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "dbo",
                table: "MyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                schema: "dbo",
                table: "MyUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gender",
                schema: "dbo",
                table: "MyUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "dbo",
                table: "MyUser");

            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "dbo",
                table: "MyUser");

            migrationBuilder.DropColumn(
                name: "Job",
                schema: "dbo",
                table: "MyUser");

            migrationBuilder.DropColumn(
                name: "gender",
                schema: "dbo",
                table: "MyUser");
        }
    }
}
