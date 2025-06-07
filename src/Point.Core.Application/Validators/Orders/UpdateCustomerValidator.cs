using FluentValidation;
using Point.Core.Application.Handlers.Orders;

namespace Point.Core.Application.Validators.Orders
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerValidator() 
        {
            RuleFor(x => x.Name)
                    .NotEmpty()
                    .MaximumLength(100);
        }
    }
}
