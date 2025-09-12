using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandPhotoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_BrandId",
                table: "Photos",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Brands_BrandId",
                table: "Photos",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Brands_BrandId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_BrandId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Photos");
        }
    }
}
