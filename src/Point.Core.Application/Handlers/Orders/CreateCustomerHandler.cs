using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record CreateCustomerRequest(
        string Name)
        : IRequest<int>;

    public class CreateCustomerHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateCustomerRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.Customers.AnyAsync(t => t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Customer already exist.");
            }

            var customer = new Customer
            {
                Name = request.Name
            };

            _pointDbContext.Customers.Add(customer);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return customer.Id;
        }
    }
}
