using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.SubTotal)
                .GreaterThan(0);

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Total)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .Must(items => items?.Count > 0)
                .WithMessage("At least one Item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new CreateOrderItemValidator())
                .When(x => x.Items != null);
        }
    }
}
