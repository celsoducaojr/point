using FluentValidation;
using Point.Core.Application.Handlers.Order;

namespace Point.Core.Application.Validators
{
    public class UpdateSupplierValidator : AbstractValidator<UpdateSupplierRequest>
    {
        public UpdateSupplierValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Remarks)
                .MaximumLength(250)
                .When(x => x.Remarks != null);
        }
    }
}
