using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities.Stocks;
using Point.Core.Domain.Enums;

namespace Point.Core.Application.Handlers.Stocks
{
    public sealed record UpdateStockItemRequest(
        int StockItemId,
        StockHistoryType Type,
        int Quantity,
        string? Remarks) 
        : IRequest<Unit>;

    public class UpdateStockItemHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateStockItemRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateStockItemRequest request, CancellationToken cancellationToken)
        {
            var stock = await _pointDbContext.StockItems
                    .Include(stock => stock.Histories).FirstOrDefaultAsync(stock => stock.Id == request.StockItemId, cancellationToken)
                    ?? throw new NotFoundException($"Stock not found.");

            if (request.Type == StockHistoryType.Removal && stock.Quantity < request.Quantity)
            {
                throw new DomainException("Insufficient stock quantity.");
            }

            stock.Quantity = request.Type == StockHistoryType.Addition
                ? stock.Quantity + request.Quantity
                : stock.Quantity - request.Quantity;
            
            stock.Histories.Add(new StockHistory
            {
                QuantityChanged = request.Type == StockHistoryType.Addition
                    ? request.Quantity
                    : -request.Quantity,
                QuantityAfterChange = stock.Quantity,
                Type = request.Type,
                Remarks = request.Remarks
            });

            _pointDbContext.StockItems.Update(stock);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
