using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator() 
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

            RuleFor(x => x.PaymentTerm)
                .IsInEnum()
                .When(x => x.PaymentTerm != null);

            RuleFor(x => x.Payment)
                .SetValidator(new CreatePaymentValidator())
                .When(x => x.Payment != null);
        }
    }

    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemRequest>
    {
        public CreateOrderItemValidator() 
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Discount)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Total)
                .GreaterThan(0);
        }

    }

    public class CreatePaymentValidator : AbstractValidator<CreatePaymentRequest?>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Mode)
                .IsInEnum();

            RuleFor(x => x.Reference)
                .MaximumLength(50)
                .When(x => x.Reference != null);

            RuleFor(x => x.Remarks)
                .MaximumLength(250)
                .When(x => x.Remarks != null);
        }
    }
}
