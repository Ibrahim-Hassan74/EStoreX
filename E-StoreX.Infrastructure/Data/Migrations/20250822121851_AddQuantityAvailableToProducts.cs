using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityAvailableToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantityAvailable",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"),
                column: "UpdatedAt",
                value: new DateTime(2025, 8, 22, 12, 18, 50, 573, DateTimeKind.Utc).AddTicks(3055));
            migrationBuilder.Sql("UPDATE Products SET QuantityAvailable = 20");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityAvailable",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"),
                column: "UpdatedAt",
                value: new DateTime(2025, 8, 21, 16, 53, 6, 220, DateTimeKind.Utc).AddTicks(1683));
        }
    }
}
