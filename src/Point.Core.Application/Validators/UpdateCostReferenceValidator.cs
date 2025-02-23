using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateCostReferenceValidator : AbstractValidator<UpdateCostReferenceRequest>
    {
        public UpdateCostReferenceValidator() 
        {
            RuleFor(x => x.InitialAmount)
                .GreaterThan(0);

            RuleFor(x => x.FinalAmount) 
                .GreaterThan(0);

            RuleForEach(x => x.Variations)
                .ChildRules(variation =>
                {
                    variation.RuleFor(x => x.Amount)
                        .GreaterThan(0)
                        .When(x => x.Amount.HasValue);

                    variation.RuleFor(x => x.Percentage)
                        .GreaterThan(0)
                        .When(x => x.Percentage.HasValue);

                    variation.RuleFor(x => x.Remarks)
                        .MaximumLength(250)
                        .When(x => x.Remarks != null);
                })
                .When(x => x.Variations?.Count > 0);
        }
    }
}
