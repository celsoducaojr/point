using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateItemUnitRequest(
        int ItemId,
        int UnitId,
        string? ItemCode,
        decimal RetialPrice,
        decimal WholeSalePrice,
        string? PriceCode,
        string? Remarks)
        : IRequest<int>;
    public class CreateItemUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateItemUnitRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateItemUnitRequest request, CancellationToken cancellationToken)
        {
            //if (!await _pointDbContext.Item
            //    .FirstOrDefaultAsync(s => s.Id == request.ItemId, cancellationToken))
            //    ?? throw new NotFoundException("Item not found.");

            //var item = (await _pointDbContext.Item
            //    .Include(s => s.Tags)
            //    .FirstOrDefaultAsync(s => s.Id == request.UnitId, cancellationToken))
            //    ?? throw new NotFoundException("Unit not found.");

            return 1;
        }
    }
}
