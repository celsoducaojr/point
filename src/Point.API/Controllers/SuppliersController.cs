using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Order;

namespace Point.API.Controllers
{
    public class SuppliersController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateSupplierRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        {
            var request = new UpdateSupplierRequest(
                id, dto.Name, dto.Remarks, dto.Tags);

            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var supplier = await _pointDbContext.Supplier
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new NotFoundException("Supplier not found.");

            return Results.Ok(supplier.Adapt<GetSupplierResponseDto>());
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            var suppliers = await _pointDbContext.Supplier
                .Include(s => s.Tags)
                .ToListAsync();

            return Results.Ok(suppliers.Adapt<List<GetSupplierResponseDto>>());
        }
    }
}
