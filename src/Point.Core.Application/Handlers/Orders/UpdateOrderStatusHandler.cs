using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateOrderStatusRequest(
      int Id,
      OrderStatus Status,
      PaymentTerm? PaymentTerm = null)
      : IRequest<Unit>;

    public class UpdateOrderStatusHandler(IPointDbContext pointDbContext, IOrderService orderService) : IRequestHandler<UpdateOrderStatusRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;

        public async Task<Unit> Handle(UpdateOrderStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Order not found.");

            if (!_orderService.IsStatusChangeAllowed(order.Status, request.Status))
            {
                throw new DomainException($"Order Status update from '{order.Status}' to '{request.Status}' is now allowed.");
            }

            if (request.Status == OrderStatus.Released && !request.PaymentTerm.HasValue)
            {
                throw new DomainException($"Payment Term is required to Release an Order.");
            }

            order.Status = request.Status;
            order.PaymentTerm = request.PaymentTerm;
            if (request.Status == OrderStatus.Released) order.Released = DateTime.Now;

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
