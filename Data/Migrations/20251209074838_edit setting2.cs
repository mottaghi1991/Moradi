using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editsetting2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ins",
                schema: "dbo",
                table: "Settings",
                newName: "WorkTime");

            migrationBuilder.AddColumn<string>(
                name: "FooterDescript",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainBanner",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainBannerAddress",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterDescript",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MainBanner",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MainBannerAddress",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SiteName",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "WorkTime",
                schema: "dbo",
                table: "Settings",
                newName: "Ins");
        }
    }
}
