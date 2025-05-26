using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class PatchItemUnitsValidator : AbstractValidator<PatchItemUnitsRequest>
    {
        public PatchItemUnitsValidator()
        {
            RuleFor(x => x.data)
                .NotEmpty();

            RuleForEach(x => x.data)
                .ChildRules(itemUnit =>
                {
                    itemUnit.RuleFor(x => x.ItemCode)
                        .MaximumLength(50)
                        .When(x => x.ItemCode != null);

                    itemUnit.RuleFor(x => x.PriceCode)
                        .MaximumLength(50)
                        .When(x => x.PriceCode != null);

                    itemUnit.RuleForEach(x => x.Prices)
                       .ChildRules(price =>
                       {
                           price.RuleFor(x => x.Amount)
                               .GreaterThanOrEqualTo(0);
                       })
                       .When(x => x.Prices?.Count > 0);

                    itemUnit.RuleFor(x => x.Prices)
                        .Must(HasUniquePriceTypes)
                        .When(x => x.Prices?.Count > 0)
                        .WithMessage("'Prices' must be unique.");
                })
                .When(x => x.data?.Count > 0);
        }

        private bool HasUniquePriceTypes(List<CreatePriceRequest>? price)
        {
            return price?.Select(i => i.PriceTypeId).Distinct().Count() == price.Count;
        }
    }
}
