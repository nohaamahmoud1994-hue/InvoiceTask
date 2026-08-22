using System.Text.Json.Serialization;

namespace Invoice.Core.Models
{

    public class TaxTotal
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        [JsonIgnore]
        public Invoices? Invoice { get; set; }
        public string TaxType { get; set; } = null!;

        public decimal Amount { get; set; }
    }
}
