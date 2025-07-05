using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateOrderRequest(
        int Id,
        int? CustomerId,
        decimal SubTotal,
        decimal Discount,
        decimal Total,
        List<CreateOrderItemRequest> Items)
        : IRequest<Unit>;

    public class UpdateOrderHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateOrderRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders
                .Include(order => order.Items).FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Order not found.");

            _pointDbContext.OrderItems.RemoveRange(order.Items);

            var orderItems = new List<OrderItem>();
            foreach (var orderItem in request.Items)
            {
                var itemUnit = await _pointDbContext.ItemUnits.FindAsync(orderItem.ItemUnitId, cancellationToken)
                    ?? throw new NotFoundException($"Item Unit not found: {orderItem.ItemUnitId}");
                var item = await _pointDbContext.Items.FindAsync(itemUnit.ItemId, cancellationToken);
                var unit = await _pointDbContext.Units.FindAsync(itemUnit.UnitId, cancellationToken);

                orderItems.Add(new OrderItem
                {
                    ItemUnitId = orderItem.ItemUnitId,
                    ItemName = item.Name,
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    Quantity = orderItem.Quantity,
                    Price = orderItem.Price,
                    Discount = orderItem.Discount,
                    Total = orderItem.Total
                });
            }

            order.CustomerId = request.CustomerId;
            order.SubTotal = request.SubTotal;
            order.Discount = request.Discount;
            order.Total = request.Total;
            order.Items = orderItems;

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
