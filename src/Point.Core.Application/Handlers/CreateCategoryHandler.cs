using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateCategoryRequest(
        string Name)
        : IRequest<IResult>;
    public class CreateCategoryHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateCategoryRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.Category.AnyAsync(c => c.Name == request.Name))
            {
                throw new DomainException("Category already exist.");
            }

            var category = new Category
            {
                Name = request.Name
            };

            _pointDbContext.Category.Add(category);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { category.Id, created = DateTime.Now });
        }
    }
}
