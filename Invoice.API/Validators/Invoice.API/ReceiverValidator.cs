using FluentValidation;
using Invoice.Core.Models;

namespace Invoice.API.Validators
{
    public class ReceiverValidator : AbstractValidator<Receiver>
    {
        public ReceiverValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .Must(type =>
                    type == "B" ||
                    type == "P" ||
                    type == "F")
                .WithMessage("Receiver Type must be 'B', 'P', or 'F'.");

            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Receiver Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Receiver Name is required.");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Receiver Address is required.");

            When(x => x.Address != null, () =>
            {
                RuleFor(x => x.Address.Country)
                    .NotEmpty()
                    .WithMessage("Receiver Country is required.");

                RuleFor(x => x.Address.Governate)
                    .NotEmpty()
                    .WithMessage("Receiver Governate is required.");

                RuleFor(x => x.Address.RegionCity)
                    .NotEmpty()
                    .WithMessage("Receiver RegionCity is required.");

                RuleFor(x => x.Address.Street)
                    .NotEmpty()
                    .WithMessage("Receiver Street is required.");

                RuleFor(x => x.Address.BuildingNumber)
                    .NotEmpty()
                    .WithMessage("Receiver BuildingNumber is required.");
            });
        }
    }
}