using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

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
        : IRequest;
    public class UpdateItemUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateItemUnitRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task Handle(UpdateItemUnitRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = (await _pointDbContext.ItemUnit.FindAsync(request.Id, cancellationToken))
                           ?? throw new NotFoundException("Item Unit not found.");

            if (await _pointDbContext.Item.FindAsync(request.ItemId, cancellationToken) == null)
            {
                throw new NotFoundException("Item not found.");
            }

            if (await _pointDbContext.Unit.FindAsync(request.UnitId) == null)
            {
                throw new NotFoundException("Unit not found.");
            }

            if (await _pointDbContext.ItemUnit.AnyAsync(i => i.Id != request.Id && i.ItemId == request.ItemId && i.UnitId == request.UnitId, cancellationToken))
            {
                throw new DomainException("Item Unit already exist.");
            }

            itemUnit.ItemId = request.ItemId;
            itemUnit.UnitId = request.UnitId;
            itemUnit.ItemCode = request.ItemCode;
            itemUnit.RetailPrice = request.RetailPrice;
            itemUnit.WholesalePrice = request.WholeSalePrice;
            itemUnit.PriceCode = request.PriceCode;
            itemUnit.Remarks = request.Remarks;

            _pointDbContext.ItemUnit.Update(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
