using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class editorderitemandaddbarchid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "OrderItem",
                column: "ProductBatchId",
                principalSchema: "dbo",
                principalTable: "ProductBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_ProductBatches_ProductBatchId",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_ProductBatchId",
                schema: "dbo",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ProductBatchId",
                schema: "dbo",
                table: "OrderItem");
        }
    }
}
