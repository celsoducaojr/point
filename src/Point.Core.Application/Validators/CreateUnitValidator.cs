using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class CreateUnitValidator : AbstractValidator<CreateUnitRequest>
    {
        public CreateUnitValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
