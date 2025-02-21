using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;

namespace Point.API.Controllers
{
    public class CategoriesController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCategoryRequest createTagRequest)
        {
            var id = await _mediator.Send(createTagRequest);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto dto)
        {
            await _mediator.Send(new UpdateCategoryRequest(id, dto.Name));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.Category.FindAsync(id))
                ?? throw new NotFoundException("Category not found.");

            return Ok(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.Category.ToListAsync());
        }
    }
}
