using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateTagValidator : AbstractValidator<UpdateTagRequest>
    {
        public UpdateTagValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
