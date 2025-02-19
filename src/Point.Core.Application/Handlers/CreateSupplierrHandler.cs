﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record CreateSupplierRequest(
        string Name,
        string Remarks,
        List<int> Tags)
        : IRequest<IResult>;
    public class CreateSupplierrHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateSupplierRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            if (_pointDbContext.Supplier.Any(s => s.Name == request.Name))
            {
                throw new DomainException("Supplier already exist.");
            }

            var supplier = new Supplier
            {
                Name = request.Name,
                Remarks = request.Remarks,
                Tags = request.Tags.Select(tagId => new SupplierTag { Id = tagId }).ToList()
            };

            _pointDbContext.Supplier.Add(supplier);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { supplier.Id, supplier.Created });
        }
    }
}
