using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandEntityAndRelationToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApiClients",
                keyColumn: "Id",
                keyValue: new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"));

            // 1. Create Brands table first
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            // 2. Insert a default brand
            var defaultBrandId = Guid.NewGuid();
            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[] { defaultBrandId, "Default Brand" });

            // 3. Add BrandId column to Products, nullable first
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            // 4. Update existing rows to default brand
            migrationBuilder.Sql($@"
        UPDATE Products
        SET BrandId = '{defaultBrandId}'
        WHERE BrandId IS NULL
    ");

            // 5. Alter column to non-nullable
            migrationBuilder.AlterColumn<Guid>(
                name: "BrandId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            // 6. Create index and foreign key
            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brands_BrandId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.InsertData(
                table: "ApiClients",
                columns: new[] { "Id", "ApiKey", "ClientName", "IsActive", "UpdatedAt" },
                values: new object[] { new Guid("125e2213-8691-45e9-ab60-d4bfa1367428"), "ovuPaA2bJcgksW6yONrlDYtKweqihHfGnd9pI1FMVRmCTzE7UBx03SXZ8QL5j4", "E-StoreX flutter Client", true, new DateTime(2025, 8, 22, 12, 18, 50, 573, DateTimeKind.Utc).AddTicks(3055) });
            migrationBuilder.Sql("DELETE FROM Brands WHERE Name = 'Default Brand'");
        }
    }
}
