using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class InvoiceValidator : AbstractValidator<Invoices>
    {
        public InvoiceValidator(
            IssuerValidator issuerValidator,
            ReceiverValidator receiverValidator,
            InvoiceLineValidator invoiceLineValidator)
        {
            RuleFor(x => x.DocumentType)
                .NotEmpty()
                .WithMessage("DocumentType is required.")
                .Equal("i")
                .WithMessage("DocumentType must be 'i'.");

            RuleFor(x => x.DocumentTypeVersion)
                .NotEmpty()
                .WithMessage("DocumentTypeVersion is required.")
                .Equal("1.0")
                .WithMessage("DocumentTypeVersion must be '1.0'.");

            RuleFor(x => x.DateTimeIssued)
                .Must(date =>
                    date == default ||
                    date.Kind == DateTimeKind.Utc)
                .WithMessage("DateTimeIssued must be in UTC.");

            RuleFor(x => x.DateTimeIssued)
                .Must(date =>
                    date == default ||
                    date <= DateTime.UtcNow)
                .WithMessage("DateTimeIssued cannot be in the future.");

            RuleFor(x => x.InternalId)
                .NotEmpty()
                .WithMessage("InternalId is required.");

            RuleFor(x => x.TaxpayerActivityCode)
                .NotEmpty()
                .WithMessage("TaxpayerActivityCode is required.");

            // Issuer
            RuleFor(x => x.Issuer)
                .NotNull()
                .WithMessage("Issuer is required.")
                .SetValidator(issuerValidator);

            // Receiver
            RuleFor(x => x.Receiver)
                .NotNull()
                .WithMessage("Receiver is required.")
                .SetValidator(receiverValidator);

            // Invoice Lines
            RuleFor(x => x.InvoiceLines)
                .NotNull()
                .WithMessage("InvoiceLines cannot be null.");

            RuleFor(x => x.InvoiceLines)
                .NotEmpty()
                .WithMessage("At least one invoice line is required.");

            RuleForEach(x => x.InvoiceLines)
                .SetValidator(invoiceLineValidator);

            // Optional
            RuleFor(x => x.ProformaInvoiceNumber)
                .MaximumLength(50)
                .When(x =>
                    !string.IsNullOrWhiteSpace(
                        x.ProformaInvoiceNumber));
        }
    }
}