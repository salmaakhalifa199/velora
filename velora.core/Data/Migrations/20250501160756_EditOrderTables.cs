using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace velora.core.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress_State",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAddress_State",
                table: "Order");
        }
    }
}
