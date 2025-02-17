using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class CreateTagValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
