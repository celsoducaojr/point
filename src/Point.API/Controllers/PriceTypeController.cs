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
    [Route("api/v{version:apiversion}/price-types")]
    public class PriceTypeController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePriceTypeRequest createPriceTypeRequest)
        {
            var id = await _mediator.Send(createPriceTypeRequest);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePriceTypeDto updatePriceTypeDto)
        {
            await _mediator.Send(new UpdatePriceTypeRequest(id, updatePriceTypeDto.Name, updatePriceTypeDto.DisplayIndex));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.PriceTypes.FindAsync(id))
                ?? throw new NotFoundException("Price Type not found.");

            return Ok(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.PriceTypes.OrderBy(type => type.DisplayIndex).ToListAsync());
        }
    }
}
