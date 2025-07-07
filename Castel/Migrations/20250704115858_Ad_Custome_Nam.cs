using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Castel.Migrations
{
    /// <inheritdoc />
    public partial class Ad_Custome_Nam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "SaleInvoiceMasters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ExchangeRate",
                keyColumn: "id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 4, 11, 58, 54, 378, DateTimeKind.Utc).AddTicks(1350));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "SaleInvoiceMasters");

            migrationBuilder.UpdateData(
                table: "ExchangeRate",
                keyColumn: "id",
                keyValue: 1L,
                column: "CreatedAt",
                value: new DateTime(2025, 6, 11, 8, 23, 50, 508, DateTimeKind.Utc).AddTicks(3373));
        }
    }
}
