using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class CreatePriceTypeValidator : AbstractValidator<CreatePriceTypeRequest>
    {
        public CreatePriceTypeValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
