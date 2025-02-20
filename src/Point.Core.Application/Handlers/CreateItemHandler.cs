using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateItemRequest(
        string Name,
        string Description,
        int? CategoryId,
        List<int>? Tags)
        : IRequest<IResult>;
    public class CreateItemHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateItemRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateItemRequest request, CancellationToken cancellationToken)
        {
            var category = (await _pointDbContext.Category.FindAsync(request.CategoryId, cancellationToken))
               ?? throw new NotFoundException("Category not found.");

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

            if (_pointDbContext.Item.Any(i => i.Name == request.Name && i.CategoryId == request.CategoryId))
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

            return Results.Ok(new { item.Id, item.Created });
        }
    }
}
