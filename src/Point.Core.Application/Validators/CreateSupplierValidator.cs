using FluentValidation;
using Point.Core.Application.Handlers.Order;

namespace Point.Core.Application.Validators
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierRequest>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Remarks)
                .MaximumLength(10);
        }
    }
}
