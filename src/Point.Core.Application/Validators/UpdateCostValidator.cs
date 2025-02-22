using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateCostValidator : AbstractValidator<UpdateCostRequest>
    {
        public UpdateCostValidator() 
        {
            RuleFor(x => x.InitialAmount)
                .GreaterThan(0);

            RuleFor(x => x.FinalAmount) 
                .GreaterThan(0);
        }
    }
}
