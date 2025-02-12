using MediatR;
using Microsoft.AspNetCore.Http;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;

namespace Point.Core.Application.Handlers.Order
{
    public sealed record UpdateSupplierRequest(
        int Id,
        string Name,
        string? Remarks)
        : IRequest<IResult>;
    public class UpdateSupplierHandler(IPointDbContext context) : IRequestHandler<UpdateSupplierRequest, IResult>
    {
        private readonly IPointDbContext _context = context;

        public async Task<IResult> Handle(UpdateSupplierRequest request, CancellationToken cancellationToken)
        {
            var supplier = await _context.Supplier.FindAsync(request.Id, cancellationToken);

            if (supplier == null)
            {
                throw new NotFoundException("Supplier not found.");
            }

            supplier.Name = request.Name;
            supplier.Remarks = request.Remarks;

            _context.Supplier.Update(supplier);
            await _context.SaveChangesAsync(cancellationToken);

            return Results.Ok(new { supplier.LastModified });
        }
    }
}
