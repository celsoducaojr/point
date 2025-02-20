using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;

namespace Point.API.Controllers
{
    public class ItemsController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateItemRequest request)
        {
            return await _mediator.Send(request);
        }

        //[HttpPut("{id}")]
        //public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        //{
        //    var request = new UpdateSupplierRequest(
        //        id, dto.Name, dto.Remarks, dto.Tags);

        //    return await _mediator.Send(request);
        //}

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.Item
                .Include(i => i.Tags)
                .Include(i => i.Units)
                .Include(i => i.CategoryId)
                .FirstOrDefaultAsync(i => i.Id == id))
                ?? throw new NotFoundException("Tag not found.");

            return Results.Ok(supplier);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _pointDbContext.Item
                .Include(i => i.Tags)
                .ToListAsync());
        }
    }
}
