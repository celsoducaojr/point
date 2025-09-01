using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateOrderItemStatusRequest(
        int OrderId,
        int OrderItemId,
        OrderItemStatus OrderItemStatus,
        CreateRefundRequest? Refund)
        : IRequest<Unit>;

    public sealed record CreateRefundRequest(
       PaymentMode Mode,
       string? Reference,
       string? Remarks);

    public class UpdateOrderItemStatusHandler(IPointDbContext pointDbContext, IOrderService orderService, IOrderItemService orderItemService) : IRequestHandler<UpdateOrderItemStatusRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;
        private readonly IOrderItemService _orderItemService = orderItemService;

        public async Task<Unit> Handle(UpdateOrderItemStatusRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders
                .Include(order => order.Items)
                .Include(order=> order.Refunds)
                .FirstOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken)
                
                ?? throw new NotFoundException($"Order not found.");

            var orderItem = order.Items.Where(orderItem => orderItem.Id == request.OrderItemId).FirstOrDefault()
                ?? throw new NotFoundException("Order Item not found.");
            
            if (!_orderService.IsStatusChangeAllowed(order.Status, request.OrderItemStatus))
            {
                throw new DomainException($"Order Item Status update from '{orderItem.Status}' to '{request.OrderItemStatus}' under {order.Status} Order is now allowed.");
            }

            if (!_orderItemService.IsStatusChangeAllowed(orderItem.Status, request.OrderItemStatus))
            {
                throw new DomainException($"Order Item Status update from '{orderItem.Status}' to '{request.OrderItemStatus}' is now allowed.");
            }

            // Update Order Item
            orderItem.Status = request.OrderItemStatus;

            // Update Order Total
            order.SubTotal = _orderService.GenerateTotal(order.Items);
            order.Discount = 0;
            order.Total = order.SubTotal;

            // Has Refund
            if (request.OrderItemStatus == OrderItemStatus.Refunded)
            {
                if (order.Refunds == null) order.Refunds = [];
                order.Refunds.Add(new Refund
                {
                    Amount = orderItem.Total,
                    Mode  = request.Refund.Mode,
                    Reference = request.Refund.Reference,
                    Remarks = request.Refund.Remarks,
                    OrderItemId = orderItem.Id
                });
            }

            if (!_orderService.IsValidCalculations(order))
            {
                throw new DomainException("Invalid Order calculations.");
            }

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
