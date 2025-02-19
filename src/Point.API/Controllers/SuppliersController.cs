using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Models;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Order;
using System.Threading;

namespace Point.API.Controllers
{
    public class SuppliersController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateSupplierRequest createSupplierRequest)
        {
            return await _mediator.Send(createSupplierRequest);
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
            var supplier = (await _pointDbContext.Supplier
                .Include(s => s.Tags)
                .FirstOrDefaultAsync(s => s.Id == id))
                ?? throw new NotFoundException("Supplier not found.");

            return Results.Ok(supplier);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _pointDbContext.Supplier
                .Include(s => s.Tags)
                .ToListAsync());
        }


    }
}
