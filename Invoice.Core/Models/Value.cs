namespace Invoice.Core.Models
{
    public class Value
    {
        public string CurrencySold { get; set; } = null!;

        public decimal AmountEGP { get; set; }

        public decimal? AmountSold { get; set; }

        public decimal? CurrencyExchangeRate { get; set; }
    }
}
