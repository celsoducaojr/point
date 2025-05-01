using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.PriceTypes.FindAsync(id))
                ?? throw new NotFoundException("Price Type not found.");

            return Ok(supplier);
        }
    }
}
