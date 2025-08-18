using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtColumnAtApiClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ApiClients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"),
                column: "UpdatedAt",
                value: new DateTime(2025, 8, 18, 17, 12, 17, 362, DateTimeKind.Utc).AddTicks(4819));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ApiClients");
        }
    }
}
