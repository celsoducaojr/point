using MediatR;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateOrderStatusRequest(
      int Id,
      OrderStatus OrderStatus)
      : IRequest<Unit>;


    public class UpdateOrderStatusHandler(IPointDbContext pointDbContext, IOrderService orderService) : IRequestHandler<UpdateOrderStatusRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;

        public async Task<Unit> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Order not found.");

            if (!_orderService.IsStatusChangeAllowed(order.Status, request.OrderStatus))
            {
                throw new DomainException($"Order Status update from '{order.Status}' to '{request.OrderStatus}' is now allowed.");
            }

            order.Status = request.OrderStatus;

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
