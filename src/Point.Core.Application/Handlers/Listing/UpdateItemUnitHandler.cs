using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record UpdateItemUnitRequest(
        int Id,
        int ItemId,
        int UnitId,
        string? ItemCode,
        string? CostPriceCode,
        List<CreatePriceRequest>? Prices)
        : IRequest<Unit>;
    public class UpdateItemUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateItemUnitRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateItemUnitRequest request, CancellationToken cancellationToken)
        {
            var unit = await _pointDbContext.ItemUnits
                    .Include(unit => unit.Prices).FirstOrDefaultAsync(unit => unit.Id == request.Id, cancellationToken)
                           ?? throw new NotFoundException($"Item-unit not found.");

            if (await _pointDbContext.Items.FindAsync(request.ItemId, cancellationToken) == null)
            {
                throw new NotFoundException("Item not found.");
            }

            if (await _pointDbContext.Units.FindAsync(request.UnitId) == null)
            {
                throw new NotFoundException("Unit not found.");
            }

            if (await _pointDbContext.ItemUnits.AnyAsync(i => i.Id != request.Id && i.ItemId == request.ItemId && i.UnitId == request.UnitId, cancellationToken))
            {
                throw new DomainException("Item-unit already exist.");
            }

            if (unit.Prices?.Count > 0)
            {
                _pointDbContext.Prices.RemoveRange(unit.Prices);
            }

            unit.ItemId = request.ItemId;
            unit.UnitId = request.UnitId;
            unit.ItemCode = request.ItemCode;
            unit.CostPriceCode = request.CostPriceCode;
            unit.Prices = request.Prices?.Select(price => 
                new Domain.Entities.Price 
                { 
                    Amount = price.Amount, 
                    PriceTypeId = price.PriceTypeId })
                .ToList();

            _pointDbContext.ItemUnits.Update(unit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
