using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record CreatePriceTypeRequest(
        string Name,
        int DisplayIndex)
        : IRequest<int>;

    public class CreatePriceTypeHandler(IPointDbContext pointDbContext) : IRequestHandler<CreatePriceTypeRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreatePriceTypeRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.PriceTypes.AnyAsync(c => c.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Price Type already exist.");
            }

            var priceType = new PriceType
            {
                Name = request.Name,
                DisplayIndex = request.DisplayIndex
            };

            _pointDbContext.PriceTypes.Add(priceType);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return priceType.Id;
        }
    }
}
