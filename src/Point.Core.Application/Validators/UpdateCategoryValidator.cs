using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryValidator() 
        {
            RuleFor(x => x.Name)
                   .NotEmpty()
                   .MaximumLength(30);
        }
    }
}
