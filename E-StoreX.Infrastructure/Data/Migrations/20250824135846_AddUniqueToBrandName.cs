using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueToBrandName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Brands_Name",
                table: "Brands");
        }
    }
}
