using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
{
    public class CreatePriceTypeValidator : AbstractValidator<CreatePriceTypeRequest>
    {
        public CreatePriceTypeValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.DisplayIndex)
                .GreaterThan(0);
        }
    }
}
