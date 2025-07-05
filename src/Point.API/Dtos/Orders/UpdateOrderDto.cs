using MediatR;
using Point.Core.Application.Handlers.Orders;

namespace Point.API.Dtos.Orders
{
    public sealed record UpdateOrderDto(
         int Id,
         int? CustomerId,
         decimal SubTotal,
         decimal Discount,
         decimal Total,
         List<CreateOrderItemRequest> Items);
}
