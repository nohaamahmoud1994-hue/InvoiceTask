using Invoice.Core.Models;

namespace Invoice.Core.Responses
{
    public class ValidationResponse
    {
        public bool IsValid { get; set; }

        public List<string> Errors { get; set; } = new();

        public decimal TotalSalesAmount { get; set; }

        public decimal TotalDiscountAmount { get; set; }

        public decimal NetAmount { get; set; }

        public List<TaxTotal> TaxTotals { get; set; } = new();

        public decimal TotalAmount { get; set; }
    }
}
