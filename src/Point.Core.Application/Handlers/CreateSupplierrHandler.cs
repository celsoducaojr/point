using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record CreateSupplierRequest(
        string Name,
        string Remarks)
        : IRequest<IResult>;
    public class CreateSupplierrHandler(IPointDbContext pointDbContext) : IRequestHandler<CreateSupplierRequest, IResult>
    {
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        public async Task<IResult> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = _pointDbContext.Supplier.FirstOrDefault(s => s.Name == request.Name);

            if (supplier != null)
            {
                throw new DomainException("Supplier Name already exist.");
            }

            supplier = new Supplier
            {
                Name = request.Name,
                Remarks = request.Remarks
            };

            _pointDbContext.Supplier.Add(supplier);
            await _pointDbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { supplier.Id, supplier.Created });
        }
    }
}
