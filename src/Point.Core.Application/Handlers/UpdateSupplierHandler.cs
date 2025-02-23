using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record UpdateSupplierRequest(
        int Id,
        string Name,
        string? Remarks,
        List<int>? Tags)
        : IRequest<Unit>;
    public class UpdateSupplierHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateSupplierRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = (await _pointDbContext.Supplier
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken))
                ?? throw new NotFoundException("Supplier not found.");

            if (request.Tags?.Count > 0)
            {
                var tags = await _pointDbContext.Tag
                .Where(t => request.Tags.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

                var missingTags = request.Tags.Except(tags).ToList();
                if (missingTags.Any())
                {
                    throw new NotFoundException($"Tags(s) not found: {string.Join(", ", missingTags)}");
                }
            }

            if (await _pointDbContext.Supplier.AnyAsync(s => s.Id != request.Id && s.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Supplier already exist.");
            }

            if (supplier.Tags.Count > 0)
            {
                _pointDbContext.SupplierTag.RemoveRange(supplier.Tags);
            }

            supplier.Name = request.Name;
            supplier.Remarks = request.Remarks;
            supplier.Tags = request.Tags?.Select(tagId => new Domain.Entities.SupplierTag { TagId = tagId }).ToList();

            _pointDbContext.Supplier.Update(supplier);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
