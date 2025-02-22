using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateUnitValidator : AbstractValidator<UpdateUnitRequest>
    {
        public UpdateUnitValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
