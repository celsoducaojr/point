using System.Linq;
using MediatR;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record CreateOrderRequest(
        int? CustomerId,
        decimal SubTotal,
        decimal Discount,
        decimal Total,
        List<CreateOrderItemRequest> Items,
        PaymentTerm? PaymentTerm,
        CreatePaymentRequest? Payment)
        : IRequest<string>;

    public sealed record CreateOrderItemRequest(
        int ItemUnitId,
        int Quantity,
        decimal Price,
        decimal Discount,
        decimal Total);

    public sealed record CreatePaymentRequest(
        decimal Amount,
        PaymentMode Mode,
        string? Reference,
        string? Remarks);

    public class CreateOrderHandler(IPointDbContext pointDbContext, IOrderService orderService) : IRequestHandler<CreateOrderRequest, string>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;

        public async Task<string> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            if (request.CustomerId.HasValue && await _pointDbContext.Customers.FindAsync(request.CustomerId, cancellationToken) == null)
            {
                throw new NotFoundException("Customer not found.");
            }

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

            var status = OrderStatus.New;
            List<Payment>? payments = null;
            if (request.Payment != null)
            {
                if (request.Payment.Amount == request.Total)
                {
                    status = OrderStatus.Paid;
                    payments =
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

            var order = new Order
            {
                Number = _orderService.GenerateOrderNumber(),
                CustomerId = request.CustomerId,
                SubTotal = request.SubTotal,
                Discount = request.Discount,
                Total = request.Total,
                Status = status,
                Items = orderItems,
                PaymentTerm = request.PaymentTerm,
                Payments = payments
            };

            _pointDbContext.Orders.Add(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return order.Number;
        }
    }
}
