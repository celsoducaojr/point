﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers
{
    public sealed record UpdateCategoryRequest(
        int Id,
        string Name)
        : IRequest;
    public class UpdateCategoryHandler(IPointDbContext pointDbContext) : IRequestHandler<UpdateCategoryRequest>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = (await _pointDbContext.Category.FindAsync(request.Id, cancellationToken))
               ?? throw new NotFoundException("Category not found.");

            if (await _pointDbContext.Category.AnyAsync(c => c.Id != request.Id && c.Name == request.Name))
            {
                throw new DomainException("Category already exist.");
            }

            category.Name = request.Name;

            _pointDbContext.Category.Update(category);
            await _pointDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
