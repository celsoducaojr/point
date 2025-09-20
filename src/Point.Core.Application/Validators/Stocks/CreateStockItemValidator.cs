using FluentValidation;
using Point.Core.Application.Handlers.Stocks;

namespace Point.Core.Application.Validators.Stocks
{
    public class CreateStockItemValidator : AbstractValidator<CreateStockItemRequest>
    {
        public CreateStockItemValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
