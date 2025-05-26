using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateItemUnitRequest(
        int ItemId,
        int UnitId,
        string? ItemCode,
        string? PriceCode,
        List<CreatePriceRequest>? Prices)
        : IRequest<int>;

    public sealed record CreatePriceRequest(
        int PriceTypeId,
        decimal Amount);

    public class CreateItemUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateItemUnitRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateItemUnitRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.Items.FindAsync(request.ItemId, cancellationToken) == null)
            {
                throw new NotFoundException("Item not found.");
            }

            if (await _pointDbContext.Units.FindAsync(request.UnitId) == null)
            {
                throw new NotFoundException("Unit not found.");
            }

            if (await _pointDbContext.ItemUnits.AnyAsync(i => i.ItemId == request.ItemId && i.UnitId == request.UnitId, cancellationToken))
            {
                throw new DomainException("Item-unit already exist.");
            }

            if (request.Prices?.Count > 0)
            {
                var ids = request.Prices.Select(p => p.PriceTypeId).ToList();
                var types = await _pointDbContext.PriceTypes
                .Where(t => ids.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

                var missingTags = ids.Except(types).ToList();
                if (missingTags.Any())
                {
                    throw new NotFoundException($"Price Type(s) not found: {string.Join(", ", missingTags)}");
                }
            }

            var itemUnit = new ItemUnit 
            {
                ItemId = request.ItemId,
                UnitId = request.UnitId,
                ItemCode = request.ItemCode,
                PriceCode = request.PriceCode,
                Prices = request.Prices?.Select(price => 
                    new Price 
                    {
                        Amount = price.Amount, 
                        PriceTypeId = price.PriceTypeId})
                    .ToList()
            };

            _pointDbContext.ItemUnits.Add(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return itemUnit.Id;
        }
    }
}
