

using FluentValidation;
using Invoice.API.Data;
using Invoice.Core.Interfaces;
using Invoice.Core.Models;
using Invoice.Core.Responses;
using Invoice.Core.Responses.Invoice.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Invoice.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly InvoiceDbContext _context;
        private readonly IValidator<Invoices> _invoiceValidator;
        public InvoiceService(InvoiceDbContext context,IValidator<Invoices> invoiceValidator)
        {
            _context = context;
            _invoiceValidator = invoiceValidator;
        }
        public ValidationResponse ValidateInvoice(Invoices invoice)
        {
            var response = new ValidationResponse();

            if (invoice == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invoice cannot be null.");
                return response;
            }

            // Default DateTimeIssued
            if (invoice.DateTimeIssued == default)
            {
                invoice.DateTimeIssued = DateTime.UtcNow;
            }

            // FluentValidation
            var validationResult = _invoiceValidator.Validate(invoice);

            if (!validationResult.IsValid)
            {
                response.IsValid = false;

                response.Errors = validationResult.Errors
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return response;
            }

            // Business logic / calculations
            CalculateInvoiceTotals(invoice, response);

            response.IsValid = response.Errors.Count == 0;

            return response;
        }
        private void CalculateInvoiceTotals(Invoices invoice, ValidationResponse response)
        {
            decimal totalSalesAmount = 0;
            decimal totalDiscountAmount = 0;
            decimal netAmount = 0;

            foreach (var line in invoice.InvoiceLines)
            {
                decimal calculatedSalesTotal =
                    CalculateSalesTotal(line);

                decimal calculatedDiscount =
                    CalculateDiscount(
                        line,
                        calculatedSalesTotal,
                        response);

                decimal calculatedNetTotal =
                    decimal.Round(
                        calculatedSalesTotal - calculatedDiscount,
                        5);

                // Calculate line totals
                line.SalesTotal = calculatedSalesTotal;
                line.ItemsDiscount = calculatedDiscount;
                line.NetTotal = calculatedNetTotal;

                // Calculate invoice totals
                totalSalesAmount += calculatedSalesTotal;
                totalDiscountAmount += calculatedDiscount;
                netAmount += calculatedNetTotal;
            }

            // Calculate invoice totals
            response.TotalSalesAmount =
                decimal.Round(totalSalesAmount, 5);

            response.TotalDiscountAmount =
                decimal.Round(totalDiscountAmount, 5);

            response.NetAmount =
                decimal.Round(netAmount, 5);

            // Store calculated totals in invoice
            invoice.TotalSalesAmount =
                response.TotalSalesAmount;

            invoice.TotalDiscountAmount =
                response.TotalDiscountAmount;

            invoice.NetAmount =
                response.NetAmount;

            // Calculate tax totals
            CalculateTaxTotals(invoice, response);

            // Store calculated tax totals in invoice
            invoice.TaxTotals = response.TaxTotals;

            decimal totalTax =
                response.TaxTotals.Sum(x => x.Amount);

            // Calculate total amount
            response.TotalAmount =
                decimal.Round(
                    response.NetAmount + totalTax,
                    5);

            invoice.TotalAmount =
                response.TotalAmount;
        }
        private decimal CalculateSalesTotal(InvoiceLine line)
        {
            if (line.UnitValue == null)
            {
                return 0;
            }

            decimal salesTotal =
                line.Quantity * line.UnitValue.AmountEGP;

            return decimal.Round(salesTotal, 5);
        }
        private decimal CalculateDiscount(InvoiceLine line,decimal salesTotal,ValidationResponse response)
        {
            if (line.Discount == null)
            {
                return 0;
            }

            decimal discountAmount = 0;

            // Rate is provided
            if (line.Discount.Rate.HasValue)
            {
                decimal calculatedDiscount =
                    decimal.Round(
                        salesTotal *
                        line.Discount.Rate.Value / 100,
                        5);

                // Amount is also provided
                if (line.Discount.Amount.HasValue)
                {
                    discountAmount =
                        line.Discount.Amount.Value;

                    if (decimal.Round(discountAmount, 5) !=
                        calculatedDiscount)
                    {
                        response.Errors.Add(
                            $"Discount Amount for item '{line.ItemCode}' " +
                            $"does not match the specified Discount Rate.");
                    }
                }
                else
                {
                    discountAmount =
                        calculatedDiscount;
                }
            }
            // Only Amount is provided
            else if (line.Discount.Amount.HasValue)
            {
                discountAmount =
                    line.Discount.Amount.Value;
            }

            // Discount cannot exceed SalesTotal
            if (discountAmount > salesTotal)
            {
                response.Errors.Add(
                    $"Discount Amount for item '{line.ItemCode}' " +
                    $"cannot be greater than SalesTotal.");
            }

            return decimal.Round(discountAmount, 5);
        }
        private void CalculateTaxTotals( Invoices invoice,ValidationResponse response)
        {
            var taxTotals = new Dictionary<string, decimal>();

            foreach (var line in invoice.InvoiceLines)
            {
                if (line?.TaxableItems == null)
                    continue;

                foreach (var tax in line.TaxableItems)
                {
                    if (tax == null)
                        continue;

                    decimal taxAmount = decimal.Round(tax.Amount, 5);

                    if (taxTotals.ContainsKey(tax.TaxType))
                    {
                        taxTotals[tax.TaxType] += taxAmount;
                    }
                    else
                    {
                        taxTotals[tax.TaxType] = taxAmount;
                    }
                }
            }

            response.TaxTotals = taxTotals
                .Select(x => new TaxTotal
                {
                    TaxType = x.Key,
                    Amount = decimal.Round(x.Value, 5)
                })
                .ToList();
        }
        public async Task<InvoiceOperationResponse> CreateAsync(Invoices invoice)
        {
            var response = new InvoiceOperationResponse();

            if (invoice == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invoice cannot be null.");
                return response;
            }

            // Validation + Calculation
            var validation = ValidateInvoice(invoice);

            if (!validation.IsValid)
            {
                response.IsValid = false;
                response.Errors = validation.Errors;
                return response;
            }

            // Add calculated invoice to database
            _context.Invoices.Add(invoice);

            await _context.SaveChangesAsync();

            response.IsValid = true;
            response.Message = "Invoice created successfully.";
            response.Invoice = invoice;

            return response;
        }
        public async Task<InvoiceGetAllResponse> GetAllAsync()
        {
            var response = new InvoiceGetAllResponse();

            var invoices = await _context.Invoices
                .Include(i => i.InvoiceLines)
                .Include(i => i.TaxTotals)
                .ToListAsync();

            response.IsValid = true;
            response.Message = "Invoices retrieved successfully.";
            response.Invoices = invoices;

            return response;
        }
        public async Task<InvoiceGetResponse> GetByIdAsync(int id)
        {
            var response = new InvoiceGetResponse();

            var invoice = await _context.Invoices
                .Include(i => i.InvoiceLines)
                .Include(i => i.TaxTotals)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                response.IsValid = false;
                response.Message = $"Invoice with Id {id} was not found.";
                return response;
            }

            response.IsValid = true;
            response.Message = "Invoice retrieved successfully.";
            response.Invoice = invoice;

            return response;
        }
        public async Task<InvoiceOperationResponse> UpdateAsync( int id,Invoices invoice)
        {
            var response = new InvoiceOperationResponse();

            if (invoice == null)
            {
                response.IsValid = false;
                response.Errors.Add("Invoice cannot be null.");
                return response;
            }

            var existingInvoice = await _context.Invoices
                .Include(i => i.InvoiceLines)
                .Include(i => i.TaxTotals)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (existingInvoice == null)
            {
                response.IsValid = false;
                response.Errors.Add(
                    $"Invoice with Id {id} was not found.");

                return response;
            }

            // Use the ID from the route
            invoice.Id = id;

            // Validate + calculate
            var validation = ValidateInvoice(invoice);

            if (!validation.IsValid)
            {
                response.IsValid = false;
                response.Errors = validation.Errors;

                return response;
            }

            // Update Invoice scalar properties
            _context.Entry(existingInvoice)
                .CurrentValues
                .SetValues(invoice);

            // Update Issuer
            _context.Entry(existingInvoice.Issuer)
                .CurrentValues
                .SetValues(invoice.Issuer);

            // Update Issuer Address
            _context.Entry(existingInvoice.Issuer.Address)
                .CurrentValues
                .SetValues(invoice.Issuer.Address);

            // Update Receiver
            _context.Entry(existingInvoice.Receiver)
                .CurrentValues
                .SetValues(invoice.Receiver);

            // Update Receiver Address
            if (existingInvoice.Receiver.Address != null &&
                invoice.Receiver.Address != null)
            {
                _context.Entry(existingInvoice.Receiver.Address)
                    .CurrentValues
                    .SetValues(invoice.Receiver.Address);
            }

            // Remove old InvoiceLines
            _context.InvoiceLines.RemoveRange(existingInvoice.InvoiceLines);

            // Remove old TaxTotals
            _context.TaxTotals.RemoveRange(existingInvoice.TaxTotals);

            // Set FK for new children
            foreach (var line in invoice.InvoiceLines)
            {
                line.InvoiceId = id;
            }

            foreach (var taxTotal in invoice.TaxTotals)
            {
                taxTotal.InvoiceId = id;
            }

            await _context.SaveChangesAsync();

            // Add new calculated children
            _context.InvoiceLines.AddRange(invoice.InvoiceLines);
            _context.TaxTotals.AddRange(invoice.TaxTotals);

            await _context.SaveChangesAsync();

            response.IsValid = true;
            response.Message = "Invoice updated successfully.";

            var getResponse = await GetByIdAsync(id);
            response.Invoice = getResponse.Invoice;

            return response;
        }
        public async Task<InvoiceOperationResponse> DeleteAsync(int id)
        {
            var response = new InvoiceOperationResponse();

            var invoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                response.IsValid = false;
                response.Message = $"Invoice with Id {id} was not found.";
                return response;
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            response.IsValid = true;
            response.Message = "Invoice deleted successfully.";

            return response;
        }
    }
}