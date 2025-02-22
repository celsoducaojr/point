using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateUnitRequest(
        int Id,
        string Name)
        : IRequest;
    public class UpdateUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateUnitRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        public async Task Handle(UpdateUnitRequest request, CancellationToken cancellationToken)
        {
            var unit = (await _pointDbContext.Unit.FindAsync(request.Id, cancellationToken))
                ?? throw new NotFoundException("Unit not found.");

            if (await _pointDbContext.Unit.AnyAsync(t => t.Id != request.Id && t.Name == request.Name))
            {
                throw new DomainException("Unit already exist.");
            }

            unit.Name = request.Name;

            _pointDbContext.Unit.Update(unit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
