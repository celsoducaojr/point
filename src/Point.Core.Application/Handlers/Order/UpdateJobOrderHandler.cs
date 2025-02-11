using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Enums;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record UpdateJobOrderRequest(
        int Id)
        : IRequest<IResult>;
    public class UpdateJobOrderHandler(IPointDbContext context) : IRequestHandler<UpdateJobOrderRequest, IResult>
    {
        private readonly IPointDbContext _context = context;

        public async Task<IResult> Handle(UpdateJobOrderRequest request, CancellationToken cancellationToken)
        {
            var jobOrder = await _context.JobOrder.FindAsync(request.Id, cancellationToken);

            if (jobOrder == null)
            {
                throw new NotFoundException("Job Order not found.");
            }

            jobOrder.Status = JobOrderStatus.Complete; // Test change

            _context.JobOrder.Update(jobOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { jobOrder.LastModified });
        }
    }
}
