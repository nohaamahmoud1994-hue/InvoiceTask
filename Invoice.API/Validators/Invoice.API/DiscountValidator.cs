using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class DiscountValidator : AbstractValidator<Discount>
    {
        public DiscountValidator()
        {
            When(x => x.Rate.HasValue, () =>
            {
                RuleFor(x => x.Rate!.Value)
                    .InclusiveBetween(0, 100)
                    .WithMessage(
                        "Discount Rate must be between 0 and 100.");

                RuleFor(x => x.Rate!.Value)
                    .Must(HasValidPrecision)
                    .WithMessage(
                        "Discount Rate must have no more than 5 decimal places.");
            });

            When(x => x.Amount.HasValue, () =>
            {
                RuleFor(x => x.Amount!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(
                        "Discount Amount cannot be negative.");

                RuleFor(x => x.Amount!.Value)
                    .Must(HasValidPrecision)
                    .WithMessage(
                        "Discount Amount must have no more than 5 decimal places.");
            });
        }

        private bool HasValidPrecision(decimal value)
        {
            return decimal.Round(value, 5) == value;
        }
    }
}