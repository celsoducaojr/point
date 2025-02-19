using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateTagRequest(
       int Id,
       string Name)
       : IRequest<IResult>;
    public class UpdateTagHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateTagRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
        {
            var tag = (await _pointDbContext.Tag.FindAsync(request.Id, cancellationToken))
                ?? throw new NotFoundException("Tag not found.");

            if (await _pointDbContext.Tag.AnyAsync(t => t.Id != request.Id && t.Name == request.Name))
            {
                throw new DomainException("Tag already exist.");
            }

            tag.Name = request.Name;

            _pointDbContext.Tag.Update(tag);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { lastModified = DateTime.Now });
        }
    }
}
