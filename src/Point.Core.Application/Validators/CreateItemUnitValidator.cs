using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class CreateItemUnitValidator : AbstractValidator<CreateItemUnitRequest>
    {
        public CreateItemUnitValidator() 
        {
            RuleFor(x => x.ItemCode)
                .MaximumLength(50)
                .When(x => x.ItemCode != null)
                .WithMessage("Item Code cannot exceed 50 characters.");

            RuleFor(x => x.RetailPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.WholeSalePrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PriceCode)
                .MaximumLength(50)
                .When(x => x.PriceCode != null)
                .WithMessage("Price Code cannot exceed 50 characters.");

            RuleFor(x => x.Remarks)
                .MaximumLength(250)
                .When(x => x.Remarks != null)
                .WithMessage("Remarks cannot exceed 250 characters.");
        }
    }
}
