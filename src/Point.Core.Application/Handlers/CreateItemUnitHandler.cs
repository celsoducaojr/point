﻿using MediatR;
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
        decimal RetailPrice,
        decimal WholeSalePrice,
        string? PriceCode,
        string? Remarks)
        : IRequest<int>;
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
                throw new DomainException("Item Unit already exist.");
            }

            var itemUnit = new ItemUnit 
            {
                ItemId = request.ItemId,
                UnitId = request.UnitId,
                ItemCode = request.ItemCode,
                RetailPrice = request.RetailPrice,
                WholesalePrice = request.WholeSalePrice,
                PriceCode = request.PriceCode,
                Remarks = request.Remarks
            };

            _pointDbContext.ItemUnits.Add(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return itemUnit.Id;
        }
    }
}
