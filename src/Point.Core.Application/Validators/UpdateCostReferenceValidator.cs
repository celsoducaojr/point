using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class UpdateCostReferenceValidator : AbstractValidator<UpdateCostReferenceRequest>
    {
        public UpdateCostReferenceValidator() 
        {
            RuleFor(x => x.InitialAmount)
                .GreaterThan(0);

            RuleFor(x => x.FinalAmount) 
                .GreaterThan(0);
        }
    }
}
