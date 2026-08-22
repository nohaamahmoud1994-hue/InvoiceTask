using Invoice.Core.Models;
using Invoice.Core.Responses;

namespace Invoice.Mvc.Interfaces
{
    public interface IInvoiceApiService
    {
        Task<InvoiceOperationResponse> CreateAsync(Invoices invoice);

        Task<List<Invoices>> GetAllAsync();

        Task<Invoices?> GetByIdAsync(int id);

        Task<InvoiceOperationResponse> UpdateAsync(int id, Invoices invoice);

        Task<InvoiceOperationResponse> DeleteAsync(int id);
    }
}