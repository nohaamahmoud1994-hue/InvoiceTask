

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

        public InvoiceService(InvoiceDbContext context)
        {
            _context = context;
        }
        public ValidationResponse ValidateInvoice(Invoices invoice)
        {
            var response = new ValidationResponse();

            // 1. Check if invoice is null
            if (invoice == null)
            {
                response.Errors.Add("Invoice cannot be null.");
                response.IsValid = false;
                return response;
            }

            // 2. Validate basic invoice fields
            ValidateBasicInvoice(invoice, response);

            // 3. Validate issuer
            ValidateIssuer(invoice, response);

            // 4. Validate receiver
            ValidateReceiver(invoice, response);

            // 5. Validate invoice lines
            ValidateInvoiceLines(invoice, response);

            // 6. Calculate and reconcile only if structural validation passed
            if (response.Errors.Count == 0)
            {
                CalculateInvoiceTotals(invoice, response);

            }

            // 7. Final validation result
            response.IsValid = response.Errors.Count == 0;

            return response;
        }

        private void ValidateBasicInvoice(Invoices invoice,ValidationResponse response)
        {
            // DocumentType
            if (invoice.DocumentType != "i")
            {
                response.Errors.Add(
                    "DocumentType must be 'i'.");
            }

            // DocumentTypeVersion
            if (invoice.DocumentTypeVersion != "1.0")
            {
                response.Errors.Add(
                    "DocumentTypeVersion must be '1.0'.");
            }

            // DateTimeIssued
            if (invoice.DateTimeIssued == default)
            {
                invoice.DateTimeIssued = DateTime.UtcNow;
            }
            else
            {
                // Must be UTC
                if (invoice.DateTimeIssued.Kind != DateTimeKind.Utc)
                {
                    response.Errors.Add(
                        "DateTimeIssued must be in UTC.");
                }

                // Must not be in the future
                if (invoice.DateTimeIssued > DateTime.UtcNow)
                {
                    response.Errors.Add(
                        "DateTimeIssued cannot be in the future.");
                }
            }

            // InternalId
            if (string.IsNullOrWhiteSpace(invoice.InternalId))
            {
                response.Errors.Add(
                    "InternalId is required.");
            }

            // TaxpayerActivityCode
            if (string.IsNullOrWhiteSpace(
                invoice.TaxpayerActivityCode))
            {
                response.Errors.Add(
                    "TaxpayerActivityCode is required.");
            }
        }

        private void ValidateIssuer(Invoices invoice,ValidationResponse response)
        {
            if (invoice.Issuer == null)
            {
                response.Errors.Add(
                    "Issuer is required.");
                return;
            }

            // Issuer Type
            if (string.IsNullOrWhiteSpace(invoice.Issuer.Type))
            {
                response.Errors.Add("Issuer Type is required.");
            }
            else if (invoice.Issuer.Type != "B")
            {
                response.Errors.Add("Issuer Type must be 'B'.");
            }

            if (invoice.Issuer.Address == null)
            {
                response.Errors.Add("Issuer Address is required.");
            }
            else if (string.IsNullOrWhiteSpace(invoice.Issuer.Address.BranchId))
            {
                response.Errors.Add("Issuer BranchId is required.");
            }
            // Issuer Id
            if (string.IsNullOrWhiteSpace(invoice.Issuer.Id))
            {
                response.Errors.Add("Issuer Id is required.");
            }

            // Issuer Name
            if (string.IsNullOrWhiteSpace(invoice.Issuer.Name))
            {
                response.Errors.Add("Issuer Name is required.");
            }

           
        }
        private void ValidateReceiver( Invoices invoice,ValidationResponse response)
        {
            if (invoice.Receiver == null)
            {
                response.Errors.Add("Receiver is required.");
                return;
            }

            // Receiver Type
            if (string.IsNullOrWhiteSpace(invoice.Receiver.Type))
            {
                response.Errors.Add("Receiver Type is required.");
            }
            else if (invoice.Receiver.Type != "B" &&
                     invoice.Receiver.Type != "P" &&
                     invoice.Receiver.Type != "F")
            {
                response.Errors.Add("Receiver Type must be B, P, or F.");
            }

            // Receiver Id
            if (string.IsNullOrWhiteSpace(invoice.Receiver.Id))
            {
                response.Errors.Add(
                    "Receiver Id is required.");
            }

            // Receiver Name
            if (string.IsNullOrWhiteSpace(invoice.Receiver.Name))
            {
                response.Errors.Add(
                    "Receiver Name is required.");
            }

            // Receiver Address
            if (invoice.Receiver.Address == null)
            {
                response.Errors.Add(
                    "Receiver Address is required.");
            }
        }

        private void ValidateInvoiceLines( Invoices invoice, ValidationResponse response)
        {
            if (invoice.InvoiceLines == null ||
                invoice.InvoiceLines.Count == 0)
            {
                response.Errors.Add(
                    "At least one invoice line is required.");

                return;
            }

            foreach (var line in invoice.InvoiceLines)
            {
                // Line cannot be null
                if (line == null)
                {
                    response.Errors.Add(
                        "Invoice line cannot be null.");

                    continue;
                }

                // Quantity
                if (line.Quantity <= 0)
                {
                    response.Errors.Add(
                        "Invoice line Quantity must be greater than zero.");
                }

                // ItemType
                if (line.ItemType != "GS1" &&
                    line.ItemType != "EGS")
                {
                    response.Errors.Add(
                        "Invoice line ItemType must be either 'GS1' or 'EGS'.");
                }

                // Description
                if (string.IsNullOrWhiteSpace(line.Description))
                {
                    response.Errors.Add(
                        "Invoice line Description is required.");
                }

                // ItemCode
                if (string.IsNullOrWhiteSpace(line.ItemCode))
                {
                    response.Errors.Add(
                        "Invoice line ItemCode is required.");
                }

                // UnitType
                if (string.IsNullOrWhiteSpace(line.UnitType))
                {
                    response.Errors.Add(
                        "Invoice line UnitType is required.");
                }

                // UnitValue
                if (line.UnitValue == null)
                {
                    response.Errors.Add(
                        "Invoice line UnitValue is required.");
                }
                else
                {
                    // CurrencySold
                    if (string.IsNullOrWhiteSpace(line.UnitValue.CurrencySold))
                    {
                        response.Errors.Add("Invoice line CurrencySold is required.");
                    }

                    // AmountEGP precision
                    if (HasMoreThanFiveDecimalPlaces(line.UnitValue.AmountEGP))
                    {
                        response.Errors.Add("AmountEGP must have no more than 5 decimal places.");
                    }

                    // Foreign currency
                    if (line.UnitValue.CurrencySold != "EGP")
                    {
                        if (!line.UnitValue.AmountSold.HasValue)
                        {
                            response.Errors.Add(
                                "AmountSold is required when CurrencySold is not EGP.");
                        }

                        if (!line.UnitValue.CurrencyExchangeRate.HasValue)
                        {
                            response.Errors.Add(
                                "CurrencyExchangeRate is required when CurrencySold is not EGP.");
                        }
                    }
                    else
                    {
                        if (line.UnitValue.AmountSold.HasValue)
                        {
                            response.Errors.Add(
                                "AmountSold must not have a value when CurrencySold is EGP.");
                        }

                        if (line.UnitValue.CurrencyExchangeRate.HasValue)
                        {
                            response.Errors.Add(
                                "CurrencyExchangeRate must not have a value when CurrencySold is EGP.");
                        }
                    }

                    // CurrencyExchangeRate precision
                    if (line.UnitValue.CurrencyExchangeRate.HasValue &&
                        HasMoreThanFiveDecimalPlaces(
                            line.UnitValue.CurrencyExchangeRate.Value))
                    {
                        response.Errors.Add(
                            "CurrencyExchangeRate must have no more than 5 decimal places.");
                    }
                }

                // Discount
                if (line.Discount != null)
                {
                    // Discount Rate
                    if (line.Discount.Rate.HasValue)
                    {
                        if (line.Discount.Rate.Value < 0 ||
                            line.Discount.Rate.Value > 100)
                        {
                            response.Errors.Add(
                                "Discount Rate must be between 0 and 100.");
                        }

                        if (HasMoreThanFiveDecimalPlaces(
                            line.Discount.Rate.Value))
                        {
                            response.Errors.Add(
                                "Discount Rate must have no more than 5 decimal places.");
                        }
                    }

                    // Discount Amount
                    if (line.Discount.Amount.HasValue)
                    {
                        if (line.Discount.Amount.Value < 0)
                        {
                            response.Errors.Add(
                                "Discount Amount cannot be negative.");
                        }

                        if (HasMoreThanFiveDecimalPlaces(
                            line.Discount.Amount.Value))
                        {
                            response.Errors.Add(
                                "Discount Amount must have no more than 5 decimal places.");
                        }
                    }
                }

                // TaxableItems
                if (line.TaxableItems != null &&
                    line.TaxableItems.Any())
                {
                    foreach (var tax in line.TaxableItems)
                    {
                        if (tax == null)
                        {
                            response.Errors.Add(
                                "TaxableItem cannot be null.");

                            continue;
                        }

                        // TaxType
                        if (string.IsNullOrWhiteSpace(tax.TaxType))
                        {
                            response.Errors.Add(
                                "TaxType is required.");
                        }

                        // Tax Amount
                        if (tax.Amount < 0)
                        {
                            response.Errors.Add(
                                "Tax Amount cannot be negative.");
                        }

                        if (HasMoreThanFiveDecimalPlaces(tax.Amount))
                        {
                            response.Errors.Add(
                                "Tax Amount must have no more than 5 decimal places.");
                        }

                        // Tax Rate
                        if (tax.Rate < 0 ||
                            tax.Rate > 999)
                        {
                            response.Errors.Add(
                                "Tax Rate must be between 0 and 999.");
                        }

                        if (HasMoreThanFiveDecimalPlaces(tax.Rate))
                        {
                            response.Errors.Add(
                                "Tax Rate must have no more than 5 decimal places.");
                        }
                    }

                    // TaxType must be unique inside the line
                    var taxTypes = line.TaxableItems
                        .Where(t => t != null)
                        .Select(t => t.TaxType)
                        .Where(t => !string.IsNullOrWhiteSpace(t))
                        .ToList();

                    if (taxTypes.Count != taxTypes.Distinct().Count())
                    {
                        response.Errors.Add(
                            $"TaxType cannot be repeated within invoice line '{line.ItemCode}'.");
                    }
                }
            }
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

        private bool HasMoreThanFiveDecimalPlaces(decimal value)
        {
            return decimal.Round(value, 5) != value;
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