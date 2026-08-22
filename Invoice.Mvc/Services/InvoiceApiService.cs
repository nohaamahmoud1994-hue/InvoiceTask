using Invoice.Core.Models;
using Invoice.Core.Responses;
using Invoice.Core.Responses.Invoice.Core.Responses;
using Invoice.Mvc.Interfaces;
using System.Net.Http.Json;

namespace Invoice.Mvc.Services
{
    public class InvoiceApiService : IInvoiceApiService
    {
        private readonly HttpClient _httpClient;

        public InvoiceApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // CREATE
        public async Task<InvoiceOperationResponse> CreateAsync(
            Invoices invoice)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/InvoiceApi",
                invoice);

            var result = await response.Content
                .ReadFromJsonAsync<InvoiceOperationResponse>();

            return result ?? new InvoiceOperationResponse
            {
                IsValid = false,
                Message = "No response received from API."
            };
        }

        // GET ALL
        public async Task<List<Invoices>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(
                "api/InvoiceApi");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Invoices>();
            }

            var result = await response.Content
                .ReadFromJsonAsync<InvoiceGetAllResponse>();

            return result?.Invoices ?? new List<Invoices>();
        }

        // GET BY ID
        public async Task<Invoices?> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(
                $"api/InvoiceApi/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content
                .ReadFromJsonAsync<InvoiceGetResponse>();

            return result?.Invoice;
        }

        // UPDATE
        public async Task<InvoiceOperationResponse> UpdateAsync(
            int id,
            Invoices invoice)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/InvoiceApi/{id}",
                invoice);

            var result = await response.Content
                .ReadFromJsonAsync<InvoiceOperationResponse>();

            return result ?? new InvoiceOperationResponse
            {
                IsValid = false,
                Message = "No response received from API."
            };
        }

        // DELETE
        public async Task<InvoiceOperationResponse> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/InvoiceApi/{id}");

            var result = await response.Content
                .ReadFromJsonAsync<InvoiceOperationResponse>();

            return result ?? new InvoiceOperationResponse
            {
                IsValid = false,
                Message = "No response received from API."
            };
        }
    }
}