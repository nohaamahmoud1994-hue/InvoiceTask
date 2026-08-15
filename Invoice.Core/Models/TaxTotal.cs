namespace Invoice.Core.Models
{

    public class TaxTotal
    {
        public string TaxType { get; set; } = null!;

        public decimal Amount { get; set; }
    }
}
