using Invoice.Core.Models;
using Invoice.Core.Responses;
using Invoice.Core.Responses.Invoice.Core.Responses;

namespace Invoice.Core.Interfaces
{
    public interface IInvoiceService
    {
        ValidationResponse ValidateInvoice(Invoices invoice);

        Task<InvoiceOperationResponse> CreateAsync(Invoices invoice);

        Task<InvoiceGetAllResponse> GetAllAsync();

        Task<InvoiceGetResponse> GetByIdAsync(int id);

        Task<InvoiceOperationResponse> UpdateAsync(
            int id,
            Invoices invoice);

        Task<InvoiceOperationResponse> DeleteAsync(int id);
    }
}