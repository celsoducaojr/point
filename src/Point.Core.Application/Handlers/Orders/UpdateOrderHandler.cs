using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record UpdateOrderRequest(
        int Id,
        int? CustomerId,
        decimal SubTotal,
        decimal Discount,
        decimal Total,
        List<CreateOrderItemRequest> Items,
        CreatePaymentRequest? Payment)
        : IRequest<OrderStatus>;

    public class UpdateOrderHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateOrderRequest, OrderStatus>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<OrderStatus> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders
                .Include(order => order.Items).FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Order not found.");

            if (order.Status != OrderStatus.New)
            {
                throw new DomainException($"Updating details of '{order.Status}' Order is not allowed.");
            }

            _pointDbContext.OrderItems.RemoveRange(order.Items);

            order.CustomerId = request.CustomerId;
            order.SubTotal = request.SubTotal;
            order.Discount = request.Discount;
            order.Total = request.Total;
            order.Items = [];

            foreach (var orderItem in request.Items)
            {
                var itemUnit = await _pointDbContext.ItemUnits.FindAsync(orderItem.ItemUnitId, cancellationToken)
                    ?? throw new NotFoundException($"Item Unit not found: {orderItem.ItemUnitId}");
                var item = await _pointDbContext.Items.FindAsync(itemUnit.ItemId, cancellationToken);
                var unit = await _pointDbContext.Units.FindAsync(itemUnit.UnitId, cancellationToken);

                order.Items.Add(new OrderItem
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

            if (request.Payment != null)
            {
                if (request.Payment.Amount == request.Total)
                {
                    order.Status = OrderStatus.Paid;
                    order.Released = DateTime.Now;
                    order.Payments =
                    [
                         new Payment
                         {
                             Amount = request.Payment.Amount,
                             Mode = request.Payment.Mode,
                             Reference = request.Payment.Reference,
                             Remarks = request.Payment.Remarks
                         }
                    ];
                }
                else
                {
                    throw new DomainException("Invalid payment amount.");
                }
            }

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return order.Status;
        }
    }
}
