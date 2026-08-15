using Invoice.Core.Models;
using Invoice.Core.Responses;

namespace Invoice.Mvc.Interfaces
{
    public interface IInvoiceApiService
    {
        Task<ValidationResponse?> ValidateInvoiceAsync(Invoices invoice);
    }
}