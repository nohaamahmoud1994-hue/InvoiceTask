namespace Invoice.Core.Models
{
    public class Issuer
    {
        public string Type { get; set; } = null!;

        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public IssuerAddress Address { get; set; } = new();
    }
}
