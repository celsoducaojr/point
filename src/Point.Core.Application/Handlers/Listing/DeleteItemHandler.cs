using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record DeleteItemRequest(
        int Id)
        : IRequest;
    public class DeleteItemHandler(IPointDbContext pointDbContext, IOrderService orderService) : IRequestHandler<DeleteItemRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;

        public async Task Handle(DeleteItemRequest request, CancellationToken cancellationToken)
        {
            var item = await _pointDbContext.Items.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Item not found.");

            _pointDbContext.Items.Remove(item);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
