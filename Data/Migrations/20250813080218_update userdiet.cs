using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class updateuserdiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "UserDiet",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                schema: "dbo",
                table: "UserDiet",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PaymentAuthority",
                schema: "dbo",
                table: "UserDiet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                schema: "dbo",
                table: "UserDiet",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefId",
                schema: "dbo",
                table: "UserDiet",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropColumn(
                name: "PaymentAuthority",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                schema: "dbo",
                table: "UserDiet");

            migrationBuilder.DropColumn(
                name: "PaymentRefId",
                schema: "dbo",
                table: "UserDiet");
        }
    }
}
