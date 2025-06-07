using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record UpdateUnitRequest(
        int Id,
        string Name)
        : IRequest<Unit>;
    public class UpdateUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateUnitRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        public async Task<Unit> Handle(UpdateUnitRequest request, CancellationToken cancellationToken)
        {
            var unit = await _pointDbContext.Units.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Unit not found.");

            if (await _pointDbContext.Units.AnyAsync(t => t.Id != request.Id && t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Unit already exist.");
            }

            unit.Name = request.Name;

            _pointDbContext.Units.Update(unit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
