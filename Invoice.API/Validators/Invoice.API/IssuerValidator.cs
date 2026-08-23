using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class IssuerValidator : AbstractValidator<Issuer>
    {
        public IssuerValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Issuer Type is required.")
                .Equal("B")
                .WithMessage("Issuer Type must be 'B'.");

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Issuer Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Issuer Name is required.");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Issuer Address is required.");

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address.BranchId)
                    .NotEmpty()
                    .WithMessage("Issuer BranchId is required.");

                RuleFor(x => x.Address.Country)
                    .NotEmpty()
                    .WithMessage("Issuer Country is required.");

                RuleFor(x => x.Address.Governate)
                    .NotEmpty()
                    .WithMessage("Issuer Governate is required.");

                RuleFor(x => x.Address.RegionCity)
                    .NotEmpty()
                    .WithMessage("Issuer RegionCity is required.");

                RuleFor(x => x.Address.Street)
                    .NotEmpty()
                    .WithMessage("Issuer Street is required.");

                RuleFor(x => x.Address.BuildingNumber)
                    .NotEmpty()
                    .WithMessage("Issuer BuildingNumber is required.");
            });
        }
    }
}