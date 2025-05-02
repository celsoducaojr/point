using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
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
