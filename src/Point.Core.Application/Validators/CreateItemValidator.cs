﻿using FluentValidation;
using Point.Core.Application.Handlers;

namespace Point.Core.Application.Validators
{
    public class CreateItemValidator : AbstractValidator<CreateItemRequest>
    {
        public CreateItemValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250)
                .When(x => x.Description != null);
        }
    }
}
