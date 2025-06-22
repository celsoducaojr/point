using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Listing;

namespace Point.API.Controllers.Listing
{
    [Route("api/v{version:apiversion}/categories")]
    public class CategoryController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest createTagRequest)
        {
            var id = await _mediator.Send(createTagRequest);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNameDto updateNameDto)
        {
            await _mediator.Send(new UpdateCategoryRequest(id, updateNameDto.Name));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _pointDbContext.Categories.FindAsync(id)
                ?? throw new NotFoundException("Category not found.");

            return Ok(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.Categories.ToListAsync());
        }
    }
}
