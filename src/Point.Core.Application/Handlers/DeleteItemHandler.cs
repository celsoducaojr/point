using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record DeleteItemRequest(
        int Id)
        : IRequest<IResult>;
    public class DeleteItemHandler(IPointDbContext pointDbContext) : IRequestHandler<DeleteItemRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        public async Task<IResult> Handle(DeleteItemRequest request, CancellationToken cancellationToken)
        {
            var item = (await _pointDbContext.Item
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken))
                ?? throw new NotFoundException("Item not found.");

            _pointDbContext.Item.Remove(item);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        }
    }
}
