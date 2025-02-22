using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateItemRequest(
        string Name,
        string? Description,
        int? CategoryId,
        List<int>? Tags)
        : IRequest<int>;
    public class CreateItemHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateItemRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateItemRequest request, CancellationToken cancellationToken)
        {
            if (request.CategoryId.HasValue && await _pointDbContext.Category.FindAsync(request.CategoryId, cancellationToken) == null)
            {
                throw new NotFoundException("Category not found.");
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

            if (await _pointDbContext.Item.AnyAsync(i => i.Name == request.Name && i.CategoryId == request.CategoryId, cancellationToken))
            {
                throw new DomainException("Item already exist.");
            }

            var item = new Item
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Tags = request.Tags?.Select(tagId => new ItemTag { TagId = tagId }).ToList()
            };

            _pointDbContext.Item.Add(item);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return item.Id;
        }
    }
}
