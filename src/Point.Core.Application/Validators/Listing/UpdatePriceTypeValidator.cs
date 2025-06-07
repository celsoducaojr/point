using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
{
    public class UpdatePriceTypeValidator : AbstractValidator<UpdatePriceTypeRequest>
    {
        public UpdatePriceTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.DisplayIndex)
                .GreaterThan(0);
        }
    }
}
