using Invoice.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Responses
{
    public class InvoiceGetAllResponse
    {
        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<Invoices> Invoices { get; set; } = new();
    }
}
