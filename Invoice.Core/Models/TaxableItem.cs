namespace Invoice.Core.Models
{
    public class TaxableItem
    {
        public string TaxType { get; set; } = null!;

        public decimal Amount { get; set; }

        public string? SubType { get; set; }

        public decimal Rate { get; set; }
    }
}
