using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace velora.core.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryMethods_DeliveryMethodId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryMethodId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryMethods_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_DeliveryMethods_DeliveryMethodId",
                table: "Order");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryMethodId",
                table: "Order",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_DeliveryMethods_DeliveryMethodId",
                table: "Order",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id");
        }
    }
}
