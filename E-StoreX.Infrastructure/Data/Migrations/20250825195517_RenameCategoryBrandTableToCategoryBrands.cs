using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCategoryBrandTableToCategoryBrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand");

            migrationBuilder.RenameTable(
                name: "CategoryBrand",
                newName: "CategoryBrands");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryBrand_BrandId",
                table: "CategoryBrands",
                newName: "IX_CategoryBrands_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBrands",
                table: "CategoryBrands",
                columns: new[] { "CategoryId", "BrandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrands_Brands_BrandId",
                table: "CategoryBrands",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrands_Categories_CategoryId",
                table: "CategoryBrands",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrands_Brands_BrandId",
                table: "CategoryBrands");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryBrands_Categories_CategoryId",
                table: "CategoryBrands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryBrands",
                table: "CategoryBrands");

            migrationBuilder.RenameTable(
                name: "CategoryBrands",
                newName: "CategoryBrand");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryBrands_BrandId",
                table: "CategoryBrand",
                newName: "IX_CategoryBrand_BrandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryBrand",
                table: "CategoryBrand",
                columns: new[] { "CategoryId", "BrandId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Brands_BrandId",
                table: "CategoryBrand",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryBrand_Categories_CategoryId",
                table: "CategoryBrand",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
