using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class TaxableItemValidator : AbstractValidator<TaxableItem>
    {
        public TaxableItemValidator()
        {
            RuleFor(x => x.TaxType)
                .NotEmpty()
                .WithMessage("TaxType is required.");

            RuleFor(x => x.Amount)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tax Amount cannot be negative.");

            RuleFor(x => x.Amount)
                .Must(HasValidPrecision)
                .WithMessage(
                    "Tax Amount must have no more than 5 decimal places.");

            RuleFor(x => x.Rate)
                .InclusiveBetween(0, 999)
                .WithMessage("Tax Rate must be between 0 and 999.");

            RuleFor(x => x.Rate)
                .Must(HasValidPrecision)
                .WithMessage("Tax Rate must have no more than 5 decimal places.");
        }

        private bool HasValidPrecision(decimal value)
        {
            return decimal.Round(value, 5) == value;
        }
    }
}