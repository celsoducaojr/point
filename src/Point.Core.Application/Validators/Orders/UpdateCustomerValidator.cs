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

            RuleFor(x => x.MobileNumber)
             .MaximumLength(15)
             .When(x => !string.IsNullOrEmpty(x.MobileNumber));

            RuleFor(x => x.Email)
                .MaximumLength(50)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Address)
                .MaximumLength(200)
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(RuleFor => RuleFor.Remarks)
                .MaximumLength(250)
                .When(x => !string.IsNullOrEmpty(x.Remarks));
        }
    }
}
