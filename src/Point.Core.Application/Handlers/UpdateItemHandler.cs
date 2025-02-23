using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateItemRequest(
        int Id,
        string Name,
        string? Description,
        int? CategoryId,
        List<int>? Tags)
        : IRequest<Unit>;
    public class UpdateItemHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateItemRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateItemRequest request, CancellationToken cancellationToken)
        {
            var item = (await _pointDbContext.Items
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken))
                ?? throw new NotFoundException("Item not found.");

            if (request.CategoryId.HasValue)
            {
                var category = (await _pointDbContext.Categories.FindAsync(request.CategoryId, cancellationToken))
                    ?? throw new NotFoundException("Category not found.");
            }

            if (request.Tags?.Count > 0)
            {
                var tags = await _pointDbContext.Tags
                .Where(t => request.Tags.Contains(t.Id))
                .Select(t => t.Id)
                .ToListAsync(cancellationToken);

                var missingTags = request.Tags.Except(tags).ToList();
                if (missingTags.Any())
                {
                    throw new NotFoundException($"Tag(s) not found: {string.Join(", ", missingTags)}");
                }
            }

            if (await _pointDbContext.Items.AnyAsync(i => i.Id != request.Id && i.Name == request.Name && i.CategoryId == request.CategoryId, cancellationToken))
            {
                throw new DomainException("Item already exist.");
            }

            if (item.Tags.Count > 0)
            {
                _pointDbContext.ItemTags.RemoveRange(item.Tags);
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.CategoryId = request.CategoryId;
            item.Tags = request.Tags?.Select(tagId => new Domain.Entities.ItemTag { TagId = tagId }).ToList();

            _pointDbContext.Items.Update(item);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
