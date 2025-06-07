using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record PatchItemUnitsRequest(
        List<PatchItemUnitRequest> data)
        : IRequest<Unit>;

    public sealed record PatchItemUnitRequest(
        int Id,
        string? ItemCode,
        string? CostPriceCode,
        List<CreatePriceRequest>? Prices);

    public class PatchItemUnitsHandler(IPointDbContext pointDbContext) : IRequestHandler<PatchItemUnitsRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(PatchItemUnitsRequest request, CancellationToken cancellationToken)
        {
            foreach (var u in request.data)
            {
                var unit = (await _pointDbContext.ItemUnits
                    .Include(unit => unit.Prices).FirstOrDefaultAsync(unit => unit.Id == u.Id, cancellationToken))
                           ?? throw new NotFoundException($"Item-unit with '{u.Id}' Id not found.");

                if (unit.Prices?.Count > 0)
                {
                    _pointDbContext.Prices.RemoveRange(unit.Prices); // Remove all existing prices
                }

                unit.ItemCode = u.ItemCode;
                unit.CostPriceCode = u.CostPriceCode;
                unit.Prices = u.Prices?
                    .Where(price => price.Amount > 0) // Don't save if amoun = 0
                    .Select(price =>
                    new Domain.Entities.Price
                    {
                        Amount = price.Amount,
                        PriceTypeId = price.PriceTypeId
                    })
                    .ToList();

                _pointDbContext.ItemUnits.Update(unit);
            }

            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
