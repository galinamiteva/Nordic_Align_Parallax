using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nordic_Align_V7.Migrations.InvoiceDB
{
    /// <inheritdoc />
    public partial class AddVatToInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VatNumber",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VatNumber",
                table: "Invoices");
        }
    }
}
