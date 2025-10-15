using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class edirbachname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatcheId",
                schema: "dbo",
                table: "OrderItem");

            // ✅ فقط تغییر درست: rename به ProductBatchId
            migrationBuilder.RenameColumn(
                name: "ProductBatcheId",
                schema: "dbo",
                table: "OrderItem",
                newName: "ProductBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductBatcheId",
                schema: "dbo",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatchId",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                newName: "ProductBatcheId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItem_ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                newName: "IX_OrderItem_ProductBatcheId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatcheId",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatcheId",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id");
        }
    }
}
