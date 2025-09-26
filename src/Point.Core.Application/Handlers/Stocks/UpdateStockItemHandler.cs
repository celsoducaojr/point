using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Stocks;
using Point.Core.Domain.Enums;

namespace Point.Core.Application.Handlers.Stocks
{
    public sealed record UpdateStockItemRequest(
        int ItemUnitId,
        StockUpdateType Type,
        int Quantity,
        string? Remarks) 
        : IRequest<Unit>;

    public class UpdateStockItemHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateStockItemRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateStockItemRequest request, CancellationToken cancellationToken)
        {
            var stock = await _pointDbContext.StockItems
                    .Include(stock => stock.Histories).FirstOrDefaultAsync(stock => stock.ItemUnitId == request.ItemUnitId, cancellationToken);

            if (request.Type == StockUpdateType.Removal)
            {
                if (stock == null)
                {
                    throw new NotFoundException("Stock not found.");
                }
                if (stock.Quantity < request.Quantity)
                {
                    throw new DomainException("Insufficient stock quantity.");
                }  
            }

            if (request.Type == StockUpdateType.Addition && stock == null)
            {
                stock = new StockItem
                {
                    ItemUnitId = request.ItemUnitId,
                    Quantity = request.Quantity,
                    Histories =
                    [
                        new StockHistory
                        {
                            QuantityChanged = request.Quantity,
                            QuantityAfterChange = request.Quantity,
                            Type = request.Type,
                            Remarks = request.Remarks ?? "Initial stock addition"
                        }
                    ]
                };

                await _pointDbContext.StockItems.AddAsync(stock, cancellationToken);
            }
            else // Update existing stock
            {
                stock.Quantity = request.Type == StockUpdateType.Addition
                    ? stock.Quantity + request.Quantity
                    : stock.Quantity - request.Quantity;

                stock.Histories.Add(new StockHistory
                {
                    QuantityChanged = request.Type == StockUpdateType.Addition
                        ? request.Quantity
                        : -request.Quantity,
                    QuantityAfterChange = stock.Quantity,
                    Type = request.Type,
                    Remarks = request.Remarks
                });

                _pointDbContext.StockItems.Update(stock);
            }

            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
