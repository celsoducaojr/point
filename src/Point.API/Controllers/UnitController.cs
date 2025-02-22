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
    [Route("api/v{version:apiversion}/units")]
    public class UnitController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUnitRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUnitDto dto)
        {
            await _mediator.Send(new UpdateUnitRequest(id, dto.Name));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.Unit.FindAsync(id)
                ?? throw new NotFoundException("Unit not found.");

            return Ok(unit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.Unit.ToListAsync());
        }
    }
}
