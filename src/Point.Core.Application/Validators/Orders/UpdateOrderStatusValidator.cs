using System.Xml.XPath;
using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusRequest>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.OrderStatus)
                .IsInEnum();

            RuleFor(x => x.PaymentTerm)
                .IsInEnum()
                .When(x => x.PaymentTerm.HasValue);
        }
    }
}
