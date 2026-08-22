using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Invoice.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Issuer_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_BranchId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_Governate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_RegionCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Issuer_Address_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer_Address_Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer_Address_Room = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer_Address_Landmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Issuer_Address_AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Receiver_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Governate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_RegionCity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Room = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_Landmark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver_Address_AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentTypeVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TaxpayerActivityCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseOrderReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseOrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOrderReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesOrderDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProformaInvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSalesAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExtraDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalItemsDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitValue_CurrencySold = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitValue_AmountEGP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitValue_AmountSold = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitValue_CurrencyExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValueDifference = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemsDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount_Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Discount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InternalCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceLines_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxTotals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxTotals_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxableItem",
                columns: table => new
                {
                    InvoiceLineId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxableItem", x => new { x.InvoiceLineId, x.Id });
                    table.ForeignKey(
                        name: "FK_TaxableItem_InvoiceLines_InvoiceLineId",
                        column: x => x.InvoiceLineId,
                        principalTable: "InvoiceLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceLines_InvoiceId",
                table: "InvoiceLines",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxTotals_InvoiceId",
                table: "TaxTotals",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxableItem");

            migrationBuilder.DropTable(
                name: "TaxTotals");

            migrationBuilder.DropTable(
                name: "InvoiceLines");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
