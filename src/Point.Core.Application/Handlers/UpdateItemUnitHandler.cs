using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateItemUnitRequest(
        int Id,
        int ItemId,
        int UnitId,
        string? ItemCode,
        decimal RetailPrice,
        decimal WholeSalePrice,
        string? PriceCode,
        string? Remarks)
        : IRequest<Unit>;
    public class UpdateItemUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateItemUnitRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateItemUnitRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = (await _pointDbContext.ItemUnits.FindAsync(request.Id, cancellationToken))
                           ?? throw new NotFoundException("Item Unit not found.");

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
                throw new DomainException("Item Unit already exist.");
            }

            itemUnit.ItemId = request.ItemId;
            itemUnit.UnitId = request.UnitId;
            itemUnit.ItemCode = request.ItemCode;
            itemUnit.PriceCode = request.PriceCode;

            _pointDbContext.ItemUnits.Update(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
