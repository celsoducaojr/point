using FluentValidation;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Validators.Listing
{
    public class UpdatePriceTypeDisplayIndexValidator : AbstractValidator<UpdatePriceTypeDisplayIndexRequest>
    {
        public UpdatePriceTypeDisplayIndexValidator() 
        {
            RuleFor(x => x.Indexes)
                .NotNull();
        }
    }
}
