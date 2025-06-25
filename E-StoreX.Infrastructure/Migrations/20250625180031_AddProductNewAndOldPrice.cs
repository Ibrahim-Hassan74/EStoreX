using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductNewAndOldPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Add the new column (NewPrice) and rename old one
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "OldPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "NewPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // 2. Seed Category
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "Description" },
                values: new object[]
                {
            Guid.Parse("19F389FE-8472-46FC-83EA-2440790A2067"),
            "Electronics",
            "Devices and gadgets"
                });

            // 3. Seed Product
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Description", "CategoryId", "NewPrice", "OldPrice" },
                values: new object[]
                {
            Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
            "Sample Product",
            "This is a sample product description.",
            Guid.Parse("19F389FE-8472-46FC-83EA-2440790A2067"),
            19.99m,
            0m
                });

            // 4. Seed Photo
            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[]
                {
            Guid.NewGuid(), 
            "default.jpg",
            Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890")
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: new Guid("37a11589-ef34-463c-8c24-307ac1c177e4"));

            migrationBuilder.DropColumn(
                name: "NewPrice",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "OldPrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "ImageName", "ProductId" },
                values: new object[] { new Guid("f6ab42a1-d300-4bae-bcac-b84e5f9ff31f"), "default.jpg", new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890") });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "Price",
                value: 19.99m);
        }
    }
}
