using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
using Point.API.Dtos.Stocks;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Stocks;

namespace Point.API.Controllers.Stocks
{
    [Route("api/v{version:apiversion}/stocks")]
    public class StockController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockItemRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockItemDto updateStockItemDto)
        {
            await _mediator.Send(new UpdateStockItemRequest(id, updateStockItemDto.Type, updateStockItemDto.Quantity, updateStockItemDto.Remarks));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.StockItems.FindAsync(id)
                ?? throw new NotFoundException("Stock not found.");

            return Ok(unit);
        }
    }
}
