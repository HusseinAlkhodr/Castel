using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Castel.Migrations
{
    /// <inheritdoc />
    public partial class Add_Total_to_Invoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "SaleInvoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Total",
                table: "PurchaseInvoice",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "ExchangeRate",
                keyColumn: "id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 11, 8, 23, 50, 508, DateTimeKind.Utc).AddTicks(3373));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "SaleInvoice");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "PurchaseInvoice");

            migrationBuilder.UpdateData(
                table: "ExchangeRate",
                keyColumn: "id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 10, 0, 11, 59, 513, DateTimeKind.Utc).AddTicks(9267));
        }
    }
}
