using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
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
