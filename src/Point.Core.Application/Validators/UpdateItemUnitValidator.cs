using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateItemUnitValidator : AbstractValidator<UpdateItemUnitRequest>
    {
        public UpdateItemUnitValidator()
        {
            RuleFor(x => x.ItemCode)
                .MaximumLength(50)
                .When(x => x.ItemCode != null);

            RuleFor(x => x.RetailPrice)
                .GreaterThan(0);

            RuleFor(x => x.WholeSalePrice)
                .GreaterThan(0);

            RuleFor(x => x.PriceCode)
                .MaximumLength(50)
                .When(x => x.PriceCode != null);

            RuleFor(x => x.Remarks)
                .MaximumLength(250)
                .When(x => x.Remarks != null);
        }
    }
}
