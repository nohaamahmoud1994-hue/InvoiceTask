using System.Net;
namespace Invoice.Core.Models
{
     public class Receiver
    {
        public string Type { get; set; } = null!;

        public string? Id { get; set; }

        public string? Name { get; set; }

        public ReceiverAddress? Address { get; set; }= new ();
    }
}
