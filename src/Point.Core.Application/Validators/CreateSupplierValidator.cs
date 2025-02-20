using FluentValidation;
using Point.Core.Application.Handlers.Order;

namespace Point.Core.Application.Validators
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Remarks)
                .MaximumLength(250);
        }
    }
}
