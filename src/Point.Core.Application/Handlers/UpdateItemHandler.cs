using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateItemRequest(
        int Id,
        string Name,
        string? Description,
        int? CategoryId,
        List<int>? Tags)
        : IRequest;
    public class UpdateItemHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateItemRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task Handle(UpdateItemRequest request, CancellationToken cancellationToken)
        {
            var item = (await _pointDbContext.Item
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken))
                ?? throw new NotFoundException("Item not found.");

            if (request.CategoryId.HasValue)
            {
                var category = (await _pointDbContext.Category.FindAsync(request.CategoryId, cancellationToken))
                    ?? throw new NotFoundException("Category not found.");
            }

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

            if (await _pointDbContext.Item.AnyAsync(i => i.Id != request.Id && i.Name == request.Name && i.CategoryId == request.CategoryId, cancellationToken))
            {
                throw new DomainException("Item already exist.");
            }

            if (item.Tags.Count > 0)
            {
                _pointDbContext.ItemTag.RemoveRange(item.Tags);
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.CategoryId = request.CategoryId;
            item.Tags = request.Tags?.Select(tagId => new ItemTag { TagId = tagId }).ToList();

            _pointDbContext.Item.Update(item);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
