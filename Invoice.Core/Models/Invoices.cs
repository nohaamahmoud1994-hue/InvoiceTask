namespace Invoice.Core.Models
{
    public class Invoices
    {
        public int Id { get; set; }
        public Issuer Issuer { get; set; } = new();

        public Receiver Receiver { get; set; } = new();

        public string DocumentType { get; set; } = null!;

        public string DocumentTypeVersion { get; set; } = null!;

        public DateTime DateTimeIssued { get; set; }

        public string TaxpayerActivityCode { get; set; } = null!;

        public string InternalId { get; set; } = null!;

        // Optional
        public string? PurchaseOrderReference { get; set; }

        // Optional
        public string? PurchaseOrderDescription { get; set; }

        // Optional
        public string? SalesOrderReference { get; set; }

        // Optional
        public string? SalesOrderDescription { get; set; }

        // Optional - max 50 characters according to ETA
        public string? ProformaInvoiceNumber { get; set; }

        public List<InvoiceLine> InvoiceLines { get; set; } = new();

        public decimal TotalSalesAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public decimal NetAmount { get; set; }

        public List<TaxTotal> TaxTotals { get; set; } = new();

        public decimal ExtraDiscountAmount { get; set; }

        public decimal TotalItemsDiscountAmount { get; set; }

        public decimal TotalAmount { get; set; }

        // Optional
        public DateTime? ServiceDeliveryDate { get; set; }
    }
}
