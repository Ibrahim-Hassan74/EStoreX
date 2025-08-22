using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ApiClients",
                columns: new[] { "Id", "ApiKey", "ClientName", "IsActive", "UpdatedAt" },
                values: new object[] { new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"), "ovuPaA2bJcgksW6yONrlDYtKweqihHfGnd9pI1FMVRmCTzE7UBx03SXZ8QL5j4", "E-StoreX flutter Client", true, new DateTime(2025, 8, 22, 6, 30, 30, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"));
        }
    }
}
