﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Listing
{
    public sealed record CreateUnitRequest(
        string Name)
        : IRequest<int>;

    public class CreateUnitHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateUnitRequest, int>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<int> Handle(CreateUnitRequest request, CancellationToken cancellationToken)
        {
            if (await _pointDbContext.Units.AnyAsync(t => t.Name == request.Name, cancellationToken))
            {
                throw new DomainException("Unit already exist.");
            }

            var unit = new Domain.Entities.Unit
            {
                Name = request.Name
            };

            _pointDbContext.Units.Add(unit);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return unit.Id;
        }
    }
}
