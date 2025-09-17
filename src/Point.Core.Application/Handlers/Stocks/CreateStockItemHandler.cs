using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Stocks;
using Point.Core.Domain.Enums;

namespace Point.Core.Application.Handlers.Stocks
{
    public sealed record CreateStockItemRequest(
        int ItemUnitId,
        int Quantity)
        : IRequest<int>;

    public class CreateStockItemHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateStockItemRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateStockItemRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = await _pointDbContext.ItemUnits.FindAsync(request.ItemUnitId, cancellationToken)
              ?? throw new NotFoundException("Item Unit not found.");

            if (await _pointDbContext.StockItems.AnyAsync(stock => stock.ItemUnitId == request.ItemUnitId, cancellationToken))
            {
                throw new DomainException("Stock already exist.");
            }
             
            var stockItem = new StockItem
            {
                ItemUnitId = request.ItemUnitId,
                Quantity = request.Quantity,
                Histories =
                [
                    new StockHistory
                    {
                        QuantityChanged = request.Quantity,
                        QuantityAfterChange = request.Quantity,
                        Type = StockHistoryType.Addition,
                        Remarks = "Initial stock"
                    }
                ]
            };

            _pointDbContext.StockItems.Add(stockItem);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return stockItem.Id;
        }
    }
}
