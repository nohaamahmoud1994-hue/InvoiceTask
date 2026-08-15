namespace Invoice.Core.Models
{
    public class ReceiverAddress
    {
        public string Country { get; set; } = null!;
        public string Governate { get; set; } = null!;
        public string RegionCity { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string BuildingNumber { get; set; } = null!;

        public string? PostalCode { get; set; }
        public string? Floor { get; set; }
        public string? Room { get; set; }
        public string? Landmark { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
