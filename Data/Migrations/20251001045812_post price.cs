using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class postprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "dbo",
                table: "Provinces");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "dbo",
                table: "Provinces");

            migrationBuilder.CreateTable(
                name: "PostPrices",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvicesId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleteTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostPrices_Provinces_ProvicesId",
                        column: x => x.ProvicesId,
                        principalSchema: "dbo",
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostPrices_ProvicesId",
                schema: "dbo",
                table: "PostPrices",
                column: "ProvicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostPrices",
                schema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                schema: "dbo",
                table: "Provinces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                schema: "dbo",
                table: "Provinces",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
