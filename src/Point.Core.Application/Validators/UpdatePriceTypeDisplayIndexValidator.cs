using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
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
