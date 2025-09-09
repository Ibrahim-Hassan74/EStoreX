using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStoreX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscoundDetailsToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentUsageCount",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxUsageCount",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrentUsageCount",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "MaxUsageCount",
                table: "Discounts");
        }
    }
}
