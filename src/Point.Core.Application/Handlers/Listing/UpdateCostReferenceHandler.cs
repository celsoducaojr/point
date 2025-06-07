using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record UpdateCostReferenceRequest(
        int Id,
        decimal InitialAmount,
        decimal FinalAmount,
        List<UpdateDiscountVariationRequest>? Variations)
        : IRequest<MediatR.Unit>;

    public sealed record UpdateDiscountVariationRequest(
        decimal? Amount,
        decimal? Percentage,
        string? Remarks);

    public class UpdateCostReferenceHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCostReferenceRequest, MediatR.Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<MediatR.Unit> Handle(UpdateCostReferenceRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = await _pointDbContext.ItemUnits
                .Include(i => i.CostReference)
                .Include(i => i.CostReference.Variations)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Item Unit not found");

            if (itemUnit.CostReference?.Variations?.Count > 0)
            {
                _pointDbContext.DiscountVariations.RemoveRange(itemUnit.CostReference.Variations);
            }

            itemUnit.CostReference = new CostReference
            {
                InitialAmount = request.InitialAmount,
                FinalAmount = request.FinalAmount,
                Variations = request.Variations?
                    .Select(x => new DiscountVariation
                    {
                        Amount = x.Amount,
                        Percentage = x.Percentage, 
                        Remarks = x.Remarks
                    }).ToList()
            };

            _pointDbContext.ItemUnits.Update(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return MediatR.Unit.Value;
        }
    }
}
