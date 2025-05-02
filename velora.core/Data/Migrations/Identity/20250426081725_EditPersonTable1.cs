using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace velora.core.Migrations.Identity
{
    /// <inheritdoc />
    public partial class EditPersonTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "Identity",
                table: "Users");
        }
    }
}
