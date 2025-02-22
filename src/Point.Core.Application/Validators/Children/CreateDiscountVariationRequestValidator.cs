using FluentValidation;
using Point.Core.Application.Handlers.Request.Children;

namespace Point.Core.Application.Validators.Children
{
    public class CreateDiscountVariationRequestValidator : AbstractValidator<CreateDiscountVariationRequest>
    {
        public CreateDiscountVariationRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .When(x => x.Amount.HasValue)
                .WithMessage("Amount must be greater than zero (0).");

            RuleFor(x => x.Percentage)
                .GreaterThan(0)
                .When(x => x.Percentage.HasValue)
                .WithMessage("Percentage must be greater than zero (0).");

            RuleFor(x => x.Remarks)
                .MaximumLength(250)
                .When(x => x.Remarks != null)
                .WithMessage("Remarks cannot exceed 250 characters.");
        }
    }
}
