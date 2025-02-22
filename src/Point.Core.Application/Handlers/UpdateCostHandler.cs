using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateCostRequest(
        int Id,
        decimal InitialAmount,
        decimal FinalAmount)
        : IRequest;
    public class UpdateCostHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCostRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task Handle(UpdateCostRequest request, CancellationToken cancellationToken)
        {
            var itemUnit = await _pointDbContext.ItemUnit
                .Include(i => i.Cost)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException("Item Unit not found");

            itemUnit.Cost = new Cost
            {
                InitialAmount = request.InitialAmount,
                FinalAmount = request.FinalAmount
            };

            _pointDbContext.ItemUnit.Update(itemUnit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

        }
    }
}
