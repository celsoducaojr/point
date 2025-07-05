using System.Data;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
using Point.API.Dtos.Listing;
using Point.API.Dtos.Orders;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Orders;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Enums;
using Point.Infrastructure.Persistence.Contracts;

namespace Point.API.Controllers.Orders
{
    [Route("api/v{version:apiversion}/orders")]
    public class OrderController(
        IMediator mediator, 
        IPointDbContext pointDbContext,
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _pointDbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            await _mediator.Send(new UpdateOrderRequest(id, updateOrderDto.CustomerId,
                updateOrderDto.SubTotal, updateOrderDto.Discount, updateOrderDto.Total, updateOrderDto.Items));

            return NoContent();
        }

        [HttpPut("{id}/release")]
        public async Task<IActionResult> Release([FromRoute] int id)
        {
            await _mediator.Send(new UpdateOrderStatusRequest(id, OrderStatus.Released));

            return NoContent();
        }


        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int id)
        {
            await _mediator.Send(new UpdateOrderStatusRequest(id, OrderStatus.Cancelled));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.Orders.FindAsync(id)
                ?? throw new NotFoundException("Order not found.");

            return Ok(unit);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] int? customerId = null,
            [FromQuery] List<OrderStatus>? statuses = null)
        {
            statuses ??= [.. Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()];
            var statusIds = statuses.Select(s => (int)s).ToList();

            var idsQuery = $@"
                SELECT o.Id 
                FROM Orders o";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();


            if (customerId.HasValue)
            {
                conditions.Add("o.CustomerId = @CustomerId");
                parameters.Add("CustomerId", customerId);
            }
            if (statusIds.Any())
            {
                conditions.Add("o.Status IN @StatusIds");
                parameters.Add("StatusIds", statusIds);
            }

            // Add search criteria
            if (conditions.Any())
            {
                idsQuery += " WHERE " + string.Join(" AND ", conditions);
            }
            idsQuery += " ORDER BY o.Created DESC";

            // Execute Ids query
            var ids = await _pointDbConnection.QueryAsync<int>(idsQuery, parameters);

            var pageIds = ids
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            parameters = new DynamicParameters();
            parameters.Add("Ids", pageIds);

            var pageQuery = $@"
                SELECT
                o.Id, o.Created, o.Number, o.CustomerId, o.SubTotal, o.Discount, o.Total, o.Status, o.PaymentTerm,
                c.Id, c.Name,
                oi.Id, oi.ItemUnitId, oi.ItemName, oi.UnitId, oi.UnitName, oi.Quantity, oi.Price, oi.Discount, oi.Total,
                p.Id, p.Created, p.Amount, p.Mode, p.Reference, p.Remarks
                FROM Orders o
                LEFT JOIN Customers c ON c.Id = o.CustomerId
                LEFT JOIN OrderItems oi ON oi.OrderId = o.Id
                LEFT JOIN Payments p ON p.OrderId = o.Id
                WHERE o.Id in @Ids
                ORDER BY o.Created DESC";

            // Execute page query
            var orders = await LookupAsync(pageQuery, parameters);

            return Ok(new
            {
                data = orders,
                totalCount = ids.Count(),
                page,
                pageSize
            });
        }

        #region Queries

        private async Task<IEnumerable<SearchOrderResponseDto>> LookupAsync(string query, DynamicParameters parameters)
        {
            var orderDictionary = new Dictionary<int, SearchOrderResponseDto>();
            var orders = await _pointDbConnection.QueryAsync<Order, Customer, OrderItem, Payment, SearchOrderResponseDto>(
                query,
                (order, customer, orderItem, payment) =>
                {
                    if (!orderDictionary.TryGetValue(order.Id, out var orderEntry))
                    {
                        orderEntry = new SearchOrderResponseDto
                        {
                            Id = order.Id,
                            Created = order.Created,
                            Number = order.Number,
                            Customer = customer?.Id > 0 ? customer : null,
                            SubTotal = order.SubTotal,
                            Discount = order.Discount,
                            Total = order.Total,
                            Status = order.Status,
                            Items = [],
                            PaymentTerm = order.PaymentTerm,
                            Payments = payment?.Id > 0 ? [] : null
                        };
                        orderDictionary[order.Id] = orderEntry;
                    }

                    orderEntry.Items.Add(new SearchOrderItemResponseDto
                    {
                        Id = orderItem.Id,
                        ItemUnitId = orderItem.ItemUnitId,
                        ItemName = orderItem.ItemName,
                        UnitId  = orderItem.UnitId,
                        UnitName = orderItem.UnitName,
                        Quantity = orderItem.Quantity,
                        Price = orderItem.Price,
                        Discount = orderItem.Discount,
                        Total = orderItem.Total
                    });

                    if (payment?.Id > 0)
                    {
                        orderEntry.Payments.Add(new SearchPaymentResponseDto
                        {
                            Id = payment.Id,
                            Created = payment.Created,
                            Amount = payment.Amount,
                            Mode = payment.Mode,
                            Reference = payment.Reference,
                            Remarks = payment.Remarks
                        });
                    }

                    return orderEntry;
                },
                parameters,
                splitOn: "Id"
            );

            return orders.Distinct().ToList();
        }

        #endregion
    }
}
