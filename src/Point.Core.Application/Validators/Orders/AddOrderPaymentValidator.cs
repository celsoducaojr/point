using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class AddOrderPaymentValidator : AbstractValidator<AddOrderPaymentRequest>
    {
        public AddOrderPaymentValidator() 
        {
            RuleFor(x => x.Payment)
                .NotNull();

            RuleFor(x => x.Payment)
               .SetValidator(new CreatePaymentValidator())
               .When(x => x.Payment != null);
        }
    }
}
