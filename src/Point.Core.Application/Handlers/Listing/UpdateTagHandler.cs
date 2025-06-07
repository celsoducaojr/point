using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record UpdateTagRequest(
       int Id,
       string Name)
       : IRequest<Unit>;

    public class UpdateTagHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateTagRequest, Unit>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<Unit> Handle(UpdateTagRequest request, CancellationToken cancellationToken)
        {
            var tag = await _pointDbContext.Tags.FindAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException("Tag not found.");

            if (await _pointDbContext.Tags.AnyAsync(t => t.Id != request.Id && t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Tag already exist.");
            }

            tag.Name = request.Name;

            _pointDbContext.Tags.Update(tag);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
