using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class UpdateOrderItemStatusValidator : AbstractValidator<UpdateOrderItemStatusRequest>
    {
        public UpdateOrderItemStatusValidator() 
        {
            RuleFor(x => x.OrderItemStatus)
                .IsInEnum();

            RuleFor(x => x.Refund)
                .SetValidator(new CreateRefundValidator())
                .When(x => x.Refund != null);

        }
    }

    public class CreateRefundValidator : AbstractValidator<CreateRefundRequest?>
    {
        public CreateRefundValidator()
        {
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
