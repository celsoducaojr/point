using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateTagRequest(
       string Name)
        : IRequest<int>;
    public class CreateTagHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateTagRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateTagRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.Tags.AnyAsync(t => t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Tag already exist.");
            }

            var tag = new Tag
            {
                Name = request.Name
            };

            _pointDbContext.Tags.Add(tag);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return tag.Id;
        }
    }
}
