using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Enums;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record CreateSupplierRequest(
        string Name,
        string? Remarks)
        : IRequest<IResult>;
    public class CreateSupplierrHandler(IPointDbContext context) : IRequestHandler<CreateSupplierRequest, IResult>
    {
        private readonly IPointDbContext _context = context;

        public async Task<IResult> Handle(CreateSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = new Supplier
            {
                Name = request.Name,
                Remarks = request.Remarks
            };

            _context.Supplier.Add(supplier);
            await _context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { supplier.Id, supplier.Created });
        }
    }
}
