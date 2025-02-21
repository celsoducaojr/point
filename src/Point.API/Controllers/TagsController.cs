using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;


namespace Point.API.Controllers
{
    public class TagsController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateTagRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTagDto dto)
        {
            await _mediator.Send(new UpdateTagRequest(id, dto.Name));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _pointDbContext.Tag.FindAsync(id)
                ?? throw new NotFoundException("Tag not found.");

            return Ok(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.Tag.ToListAsync());
        }
    }
}
