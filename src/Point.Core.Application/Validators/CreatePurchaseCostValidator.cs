using FluentValidation;
using Point.Core.Application.Handlers;
using Point.Core.Application.Validators.Children;

namespace Point.Core.Application.Validators
{
    public class CreatePurchaseCostValidator : AbstractValidator<CreatePurchaseCostRequest>
    {
        public CreatePurchaseCostValidator() 
        {
            RuleFor(x => x.InitialAmount)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.FinalAmount)
                .GreaterThanOrEqualTo(1);

            //RuleFor(x => x.Variations)
            //    .ForEach(x => x.SetValidator(new CreateDiscountVariationRequestValidator()))
            //    .When(x => x.Variations?.Count > 0);
        }
    }
}
