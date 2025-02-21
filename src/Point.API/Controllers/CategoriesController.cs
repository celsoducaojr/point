using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Infrastructure.Persistence;

namespace Point.API.Controllers
{
    public class CategoriesController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateCategoryRequest createTagRequest)
        {
            return await _mediator.Send(createTagRequest);
        }


        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateCategoryDto dto)
        {
            return await _mediator.Send(new UpdateCategoryRequest(
                id, dto.Name));
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.Category.FindAsync(id))
                ?? throw new NotFoundException("Category not found.");

            return Results.Ok(supplier);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _pointDbContext.Category.ToListAsync());
        }
    }
}
