using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record CreateSupplierRequest(
        string Name,
        string Remarks,
        List<int>? Tags)
        : IRequest<IResult>;
    public class CreateSupplierHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateSupplierRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            if (request.Tags?.Count > 0)
            {
                var tags = await _pointDbContext.Tag
                .Where(t => request.Tags.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

                var missingTags = request.Tags.Except(tags).ToList();
                if (missingTags.Any())
                {
                    throw new NotFoundException($"Tag(s) not found: {string.Join(", ", missingTags)}");
                }
            }
            
            if (await _pointDbContext.Supplier.AnyAsync(s => s.Name == request.Name))
            {
                throw new DomainException("Supplier already exist.");
            }

            var supplier = new Supplier
            {
                Name = request.Name,
                Remarks = request.Remarks,
                Tags = request.Tags?.Select(tagId => new SupplierTag { TagId = tagId }).ToList()
            };

            _pointDbContext.Supplier.Add(supplier);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { supplier.Id, supplier.Created });
        }
    }
}
