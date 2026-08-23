using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class ValueValidator : AbstractValidator<Value>
    {
        public ValueValidator()
        {
            RuleFor(x => x.CurrencySold)
                .NotEmpty()
                .WithMessage("Invoice line CurrencySold is required.");

            RuleFor(x => x.AmountEGP)
                .Must(HasValidPrecision)
                .WithMessage("AmountEGP must have no more than 5 decimal places.");

            When(x => x.CurrencySold != "EGP", () =>
            {
                RuleFor(x => x.AmountSold)
                    .NotNull()
                    .WithMessage("AmountSold is required when CurrencySold is not EGP.");

                RuleFor(x => x.CurrencyExchangeRate)
                    .NotNull()
                    .WithMessage( "CurrencyExchangeRate is required when CurrencySold is not EGP.");
            });

            When(x => x.CurrencySold == "EGP", () =>
            {
                RuleFor(x => x.AmountSold)
                    .Null()
                    .WithMessage("AmountSold must not have a value when CurrencySold is EGP.");
                RuleFor(x => x.CurrencyExchangeRate)
                    .Null()
                    .WithMessage("CurrencyExchangeRate must not have a value when CurrencySold is EGP.");
            });

            When(x => x.CurrencyExchangeRate.HasValue, () =>
            {
                RuleFor(x => x.CurrencyExchangeRate!.Value)
                    .Must(HasValidPrecision)
                    .WithMessage("CurrencyExchangeRate must have no more than 5 decimal places.");
            });
        }

        private bool HasValidPrecision(decimal value)
        {
            return decimal.Round(value, 5) == value;
        }
    }
}