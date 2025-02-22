using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;

namespace Point.API.Controllers
{
    [Route("api/v{version:apiversion}/item-units")]
    public class ItemUnitController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateItemUnitRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemUnitDto dto)
        {
            await _mediator.Send(new UpdateItemUnitRequest(
                id, dto.ItemId, dto.UnitId, dto.ItemCode, dto.RetailPrice, dto.WholeSalePrice, dto.PriceCode, dto.Remarks));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var itemUnit = await _pointDbContext.ItemUnit
                .Include(i => i.Cost)
                .FirstOrDefaultAsync(i => i.Id == id)
                ?? throw new NotFoundException("Item Unit not found.");

            return Ok(itemUnit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.ItemUnit.ToListAsync());
        }
    }
}
