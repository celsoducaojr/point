﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers
{
    public sealed record CreateCategoryRequest(
        string Name)
        : IRequest<int>;
    public class CreateCategoryHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateCategoryRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
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

            return category.Id;
        }
    }
}
