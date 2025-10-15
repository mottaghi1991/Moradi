using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addsendmethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                schema: "dbo",
                table: "ProductBatches",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<int>(
                name: "AloPeykOrderId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AloPeykOrderToken",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AloPeykStatus",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AloPeykTrackingUrl",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMethod",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AloPeykOrderId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AloPeykOrderToken",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AloPeykStatus",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AloPeykTrackingUrl",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeliveryMethod",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreateDate",
                schema: "dbo",
                table: "ProductBatches",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
