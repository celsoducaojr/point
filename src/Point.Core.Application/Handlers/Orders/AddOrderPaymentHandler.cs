using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;
using Point.Core.Domain.Services;

namespace Point.Core.Application.Handlers.Orders
{
    public sealed record AddOrderPaymentRequest(
        int Id,
        CreatePaymentRequest Payment)
        : IRequest<OrderStatus>;

    public class AddOrderPaymentHandler(IPointDbContext pointDbContext, IOrderService orderService) : IRequestHandler<AddOrderPaymentRequest, OrderStatus>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IOrderService _orderService = orderService;

        public async Task<OrderStatus> Handle(AddOrderPaymentRequest request, CancellationToken cancellationToken)
        {
            var order = await _pointDbContext.Orders
                .Include(order => order.Payments).FirstOrDefaultAsync(order => order.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Order not found.");

            if (order.Status == OrderStatus.New || order.Status == OrderStatus.Cancelled
                || order.Status == OrderStatus.Paid || order.Status == OrderStatus.Refunded)
            {
                throw new DomainException($"Adding payment to {order.Status} Order is not allowed.");
            }

            var balance = _orderService.GenerateBalance(order);

            if (request.Payment.Amount > balance)
            {
                throw new DomainException("Invalid payment amount value.");
            }

            order.Payments ??= [];
            order.Payments.Add(new Payment
            {
                Amount = request.Payment.Amount,
                Mode = request.Payment.Mode,
                Reference = request.Payment.Reference,
                Remarks = request.Payment.Remarks
            });

            order.Status = OrderStatus.PartiallyPaid;
            if (request.Payment.Amount == balance) order.Status = OrderStatus.Paid;

            _pointDbContext.Orders.Update(order);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return order.Status;
        }
    }
}
