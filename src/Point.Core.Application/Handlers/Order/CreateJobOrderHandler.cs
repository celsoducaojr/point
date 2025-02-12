using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Entities.Enums;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record CreateJobOrderRequest()
        : IRequest<IResult>;
    public class CreateJobOrderHandler(IPointDbContext context) : IRequestHandler<CreateJobOrderRequest, IResult>
    {
        private readonly IPointDbContext _context = context;

        public async Task<IResult> Handle(CreateJobOrderRequest request, CancellationToken cancellationToken)
        {
            var jobOrder = new Sale
            {
                Status = SaleStatus.Pending
            };

            _context.Sale.Add(jobOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { jobOrder.Id, jobOrder.Created });
        }
    }
}
