using Invoice.Core.Models;
using Invoice.Core.Responses;

namespace Invoice.Core.Interfaces
{
    public interface IInvoiceService
    {
        ValidationResponse ValidateInvoice(Invoices invoice);
    }
}
