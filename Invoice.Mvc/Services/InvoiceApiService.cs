using Invoice.Core.Models;
using Invoice.Core.Responses;
using Invoice.Mvc.Interfaces;

namespace Invoice.Mvc.Services
{
    public class InvoiceApiService : IInvoiceApiService
    {
        private readonly HttpClient _httpClient;

        public InvoiceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ValidationResponse?> ValidateInvoiceAsync(Invoices invoice)
        {
            var response = await _httpClient.PostAsJsonAsync("api/InvoiceApi/validate",invoice);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ValidationResponse>();
        }
    }
}
