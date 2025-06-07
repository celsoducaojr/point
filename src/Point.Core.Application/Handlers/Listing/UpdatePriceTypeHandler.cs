using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record UpdatePriceTypeRequest(
        int Id,
        string Name,
        int DisplayIndex)
        : IRequest<Unit>;

    public class UpdatePriceTypeHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdatePriceTypeRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdatePriceTypeRequest request, CancellationToken cancellationToken)
        {
            var priceType = await _pointDbContext.PriceTypes.FindAsync(request.Id, cancellationToken)
               ?? throw new NotFoundException("Price Type not found.");

            if (await _pointDbContext.PriceTypes.AnyAsync(c => c.Id != request.Id && c.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Price Type already exist.");
            }

            priceType.Name = request.Name;
            priceType.DisplayIndex = request.DisplayIndex;

            _pointDbContext.PriceTypes.Update(priceType);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
