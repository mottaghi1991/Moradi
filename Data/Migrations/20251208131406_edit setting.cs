using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editsetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aboute",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "BackgroundImage",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Birthday",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Linkedin",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Tweeter",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "jobs",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "ProfileImage",
                schema: "dbo",
                table: "Settings",
                newName: "Number2");

            migrationBuilder.RenameColumn(
                name: "Instagram",
                schema: "dbo",
                table: "Settings",
                newName: "Number1");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ins",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapAddress",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Ins",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Logo",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "MapAddress",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Mobile",
                schema: "dbo",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "Number2",
                schema: "dbo",
                table: "Settings",
                newName: "ProfileImage");

            migrationBuilder.RenameColumn(
                name: "Number1",
                schema: "dbo",
                table: "Settings",
                newName: "Instagram");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Aboute",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundImage",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                schema: "dbo",
                table: "Settings",
                type: "datetime2",
                maxLength: 50,
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Linkedin",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Tweeter",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "jobs",
                schema: "dbo",
                table: "Settings",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
