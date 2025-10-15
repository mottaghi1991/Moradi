using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class edir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId1",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductBatchId1",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductBatchId1",
                schema: "dbo",
                table: "OrderItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductBatchId1",
                schema: "dbo",
                table: "OrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductBatchId1",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId1",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatchId1",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id");
        }
    }
}
