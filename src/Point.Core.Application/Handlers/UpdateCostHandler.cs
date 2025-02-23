using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateCostRequest(
        int Id,
        decimal InitialAmount,
        decimal FinalAmount)
        : IRequest<Unit>;
    public class UpdateCostHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCostRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateCostRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = await _pointDbContext.ItemUnit
                .Include(i => i.Cost)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Item Unit not found");

            itemUnit.Cost = new Domain.Entities.Cost
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
