using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Constants;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.API.Controllers
{
    [Route("api/v{version:apiversion}/items")]
    public class ItemController(
        IMediator mediator, 
        IPointDbContext pointDbContext, 
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _dbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateItemRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemDto dto)
        {
            await _mediator.Send(new UpdateItemRequest(
                id, dto.Name, dto.Description, dto.CategoryId, dto.Tags));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteItemRequest(id));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var itemUnit = await _pointDbContext.Items
                .FirstOrDefaultAsync(i => i.Id == id)
                ?? throw new NotFoundException("Item not found.");

            return Ok(itemUnit);
        }
    }
}
