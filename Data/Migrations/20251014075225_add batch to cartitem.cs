using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addbatchtocartitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductBatchId",
                schema: "dbo",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductBatchId1",
                schema: "dbo",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductBatchId",
                schema: "dbo",
                table: "CartItems",
                column: "ProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductBatchId1",
                schema: "dbo",
                table: "CartItems",
                column: "ProductBatchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "CartItems",
                column: "ProductBatchId",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductBatches_ProductBatchId1",
                schema: "dbo",
                table: "CartItems",
                column: "ProductBatchId1",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductBatches_ProductBatchId1",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductBatchId",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductBatchId1",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductBatchId",
                schema: "dbo",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductBatchId1",
                schema: "dbo",
                table: "CartItems");
        }
    }
}
