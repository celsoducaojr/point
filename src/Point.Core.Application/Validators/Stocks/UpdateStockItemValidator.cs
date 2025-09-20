using FluentValidation;
using Point.Core.Application.Handlers.Stocks;

namespace Point.Core.Application.Validators.Stocks
{
    public class UpdateStockItemValidator : AbstractValidator<UpdateStockItemRequest>
    {
        public UpdateStockItemValidator() 
        {
            RuleFor(x => x.Type)
               .IsInEnum();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.Remarks)
               .MaximumLength(250)
               .When(x => x.Remarks != null);
        }
    }
}
