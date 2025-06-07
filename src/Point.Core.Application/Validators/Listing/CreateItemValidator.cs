using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
{
    public class CreateItemValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => x.Description != null);

            RuleFor(x => x.Tags)
                .Must(HasUniqueTags)
                .When(x => x.Tags?.Count > 0)
                .WithMessage("'Tags' must be unique.");
        }

        private bool HasUniqueTags(List<int>? tags)
        {
            return tags.Count == tags.Distinct().Count();
        }
    }
}
