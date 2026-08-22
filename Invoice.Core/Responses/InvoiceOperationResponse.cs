using Invoice.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoice.Core.Responses
{
    public class InvoiceOperationResponse
    {
        public bool IsValid { get; set; }

        public List<string> Errors { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public Invoices? Invoice { get; set; }
    }
}
