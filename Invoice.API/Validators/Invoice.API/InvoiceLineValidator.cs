using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class InvoiceLineValidator : AbstractValidator<InvoiceLine>
    {
        public InvoiceLineValidator(
            ValueValidator valueValidator,
            DiscountValidator discountValidator,
            TaxableItemValidator taxableItemValidator)
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Invoice line cannot be null.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Invoice line Description is required.");

            RuleFor(x => x.ItemType)
                .NotEmpty()
                .WithMessage("Invoice line ItemType is required.")
                .Must(type =>
                    type == "GS1" ||
                    type == "EGS")
                .WithMessage(
                    "Invoice line ItemType must be either 'GS1' or 'EGS'.");

            RuleFor(x => x.ItemCode)
                .NotEmpty()
                .WithMessage("Invoice line ItemCode is required.");

            RuleFor(x => x.UnitType)
                .NotEmpty()
                .WithMessage("Invoice line UnitType is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(
                    "Invoice line Quantity must be greater than zero.");

            RuleFor(x => x.UnitValue)
                .NotNull()
                .WithMessage("Invoice line UnitValue is required.")
                .SetValidator(valueValidator);

            RuleFor(x => x.Discount)
                .SetValidator(discountValidator)
                .When(x => x.Discount != null);

            RuleForEach(x => x.TaxableItems)
                .NotNull()
                .WithMessage("TaxableItem cannot be null.");

            RuleForEach(x => x.TaxableItems)
                .SetValidator(taxableItemValidator);

            RuleFor(x => x.TaxableItems)
                .Must(HaveUniqueTaxTypes)
                .WithMessage(x =>
                    $"TaxType cannot be repeated within invoice line '{x.ItemCode}'.");
        }

        private bool HaveUniqueTaxTypes(
            List<TaxableItem> taxableItems)
        {
            if (taxableItems == null || taxableItems.Count == 0)
                return true;

            var taxTypes = taxableItems
                .Where(x => x != null)
                .Select(x => x.TaxType)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            return taxTypes.Count == taxTypes.Distinct().Count();
        }
    }
}