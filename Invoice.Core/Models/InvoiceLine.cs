using System.Text.Json.Serialization;

namespace Invoice.Core.Models
{

    public class InvoiceLine
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        [JsonIgnore]

        public Invoices? Invoice { get; set; }
        public string Description { get; set; } = null!;

        public string ItemType { get; set; } = null!;

        public string ItemCode { get; set; } = null!;

        public string UnitType { get; set; } = null!;

        public decimal Quantity { get; set; }

        public Value UnitValue { get; set; } = null!;

        public decimal SalesTotal { get; set; }

        public decimal Total { get; set; }

        public decimal ValueDifference { get; set; }

        public decimal TotalTaxableFees { get; set; }

        public decimal NetTotal { get; set; }

        public decimal ItemsDiscount { get; set; }

        // Optional
        public Discount? Discount { get; set; }

        // Optional - zero or more
        public List<TaxableItem> TaxableItems { get; set; } = new();

        // Optional
        public string? InternalCode { get; set; }
    }
}
