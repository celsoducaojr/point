using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateTagRequest(
       string Name)
        : IRequest<IResult>;
    public class CreateTagHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateTagRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateTagRequest request, CancellationToken cancellationToken)
        {
            if (_pointDbContext.Tag.Any(s => s.Name == request.Name))
            {
                throw new DomainException("Tag already exist.");
            }

            var tag = new Tag
            {
                Name = request.Name
            };

            _pointDbContext.Tag.Add(tag);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { tag.Id, created = DateTime.Now });
        }
    }
}
