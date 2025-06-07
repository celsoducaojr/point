using FluentValidation;
using Point.Core.Application.Handlers.Listing;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Validators.Listing
{
    public class CreateItemUnitValidator : AbstractValidator<CreateItemUnitRequest>
    {
        public CreateItemUnitValidator() 
        {
            RuleFor(x => x.ItemCode)
                .MaximumLength(50)
                .When(x => x.ItemCode != null);

            RuleFor(x => x.CostPriceCode)
                .MaximumLength(50)
                .When(x => x.CostPriceCode != null);

            RuleForEach(x => x.Prices)
               .ChildRules(price =>
               {
                   price.RuleFor(x => x.Amount)
                       .GreaterThan(0);
               })
               .When(x => x.Prices?.Count > 0);

            RuleFor(x => x.Prices)
                .Must(HasUniquePriceTypes)
                .When(x => x.Prices?.Count > 0)
                .WithMessage("'Prices' must be unique.");
        }

        private bool HasUniquePriceTypes(List<CreatePriceRequest>? price)
        {
            return price?.Select(i => i.PriceTypeId).Distinct().Count() == price.Count;
        }
    }
}
