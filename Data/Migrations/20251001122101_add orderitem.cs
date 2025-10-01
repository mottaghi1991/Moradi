using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addorderitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShippingAddres_ShippingAddressId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "dbo",
                table: "ShippingAddres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                schema: "dbo",
                table: "ShippingAddres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);



            migrationBuilder.AlterColumn<int>(
                name: "ShippingAddressId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                schema: "dbo",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentAuthority",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                schema: "dbo",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentRefId",
                schema: "dbo",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendPrice",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

      
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_ShippingAddres_ShippingAddressId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "dbo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Amount",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentAuthority",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PaymentRefId",
                schema: "dbo",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "SendPrice",
                schema: "dbo",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                schema: "dbo",
                table: "ShippingAddres",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AddressLine",
                schema: "dbo",
                table: "ShippingAddres",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingAddressId",
                schema: "dbo",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_ShippingAddres_ShippingAddressId",
                schema: "dbo",
                table: "Order",
                column: "ShippingAddressId",
                principalSchema: "dbo",
                principalTable: "ShippingAddres",
                principalColumn: "Id");
        }
    }
}
