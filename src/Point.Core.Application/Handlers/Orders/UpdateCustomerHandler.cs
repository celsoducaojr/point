using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Listing;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateCustomerRequest(
      int Id,
      string Name)
      : IRequest<Unit>;

    public class UpdateCustomerHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCustomerRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _pointDbContext.Customers.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Customer not found.");

            if (await _pointDbContext.Customers.AnyAsync(t => t.Id != request.Id && t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Customer already exist.");
            }

            customer.Name = request.Name;

            _pointDbContext.Customers.Update(customer);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
