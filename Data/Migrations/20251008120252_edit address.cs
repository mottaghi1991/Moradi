using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                schema: "dbo",
                table: "ShippingAddres",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                schema: "dbo",
                table: "ShippingAddres",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                schema: "dbo",
                table: "ShippingAddres");

            migrationBuilder.DropColumn(
                name: "Longitude",
                schema: "dbo",
                table: "ShippingAddres");
        }
    }
}
