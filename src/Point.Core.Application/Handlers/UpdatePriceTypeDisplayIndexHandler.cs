using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdatePriceTypeDisplayIndexRequest(
        Dictionary<int, int> Indexes)
        : IRequest<Unit>;

    public class UpdatePriceTypeDisplayIndexHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdatePriceTypeDisplayIndexRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        public async Task<Unit> Handle(UpdatePriceTypeDisplayIndexRequest request, CancellationToken cancellationToken)
        {
            try
            {
                await _pointDbContext.PriceTypes.ForEachAsync(type =>
                {
                    type.DisplayIndex = request.Indexes[type.Id];
                }, cancellationToken: cancellationToken);
            }
            catch
            {
                throw new DomainException("Invalid/Incomplete Price Type Ids/Indexes.");
            }
           
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
