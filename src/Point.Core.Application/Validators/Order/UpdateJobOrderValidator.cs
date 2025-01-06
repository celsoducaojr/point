using FluentValidation;
using Point.Core.Application.Handlers.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Point.Core.Application.Validators.Order
{
    public class UpdateJobOrderValidator : AbstractValidator<UpdateJobOrderRequest>
    {
        public UpdateJobOrderValidator()
        {
            // TODO: Add rules
        }
    }
}
