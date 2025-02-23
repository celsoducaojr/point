using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateCostReferenceRequest(
        int Id,
        decimal InitialAmount,
        decimal FinalAmount)
        : IRequest<Unit>;
    public class UpdateCostReferenceHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCostReferenceRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateCostReferenceRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = await _pointDbContext.ItemUnit
                .Include(i => i.CostReference)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Item Unit not found");

            itemUnit.CostReference = new Domain.Entities.CostReference
            {
                InitialAmount = request.InitialAmount,
                FinalAmount = request.FinalAmount
            };

            _pointDbContext.ItemUnit.Update(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
