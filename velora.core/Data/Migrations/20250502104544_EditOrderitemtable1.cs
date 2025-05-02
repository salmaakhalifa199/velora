using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace velora.core.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderitemtable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ItemOrdered_ProductName",
                table: "OrderItem",
                newName: "OrderItem_ProductName");

            migrationBuilder.RenameColumn(
                name: "ItemOrdered_ProductId",
                table: "OrderItem",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ItemOrdered_PictureUrl",
                table: "OrderItem",
                newName: "OrderItem_PictureUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItem",
                newName: "ItemOrdered_ProductId");

            migrationBuilder.RenameColumn(
                name: "OrderItem_ProductName",
                table: "OrderItem",
                newName: "ItemOrdered_ProductName");

            migrationBuilder.RenameColumn(
                name: "OrderItem_PictureUrl",
                table: "OrderItem",
                newName: "ItemOrdered_PictureUrl");
        }
    }
}
