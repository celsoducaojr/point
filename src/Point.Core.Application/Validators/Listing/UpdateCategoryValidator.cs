using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
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
