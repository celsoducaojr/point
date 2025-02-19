using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Dtos;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;


namespace Point.API.Controllers
{
    public class TagsController(IMediator mediator, IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbConnection _pointDbConnection = pointDbConnection;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateTagRequest createTagRequest)
        {
            return await _mediator.Send(createTagRequest);
        }


        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateTagDto dto)
        {
            return await _mediator.Send(new UpdateTagRequest(
                id, dto.Name));
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var query = "SELECT * FROM Tag WHERE Id = @Id";
            var tag = await _pointDbConnection.Connection
                .QuerySingleOrDefaultAsync<Tag>(query, new { Id = id })
                ?? throw new NotFoundException("Tag not found.");

            return Results.Ok(tag);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            var tag = await _pointDbConnection.Connection
                .QueryAsync<Tag>("SELECT * FROM Tag");

            return Results.Ok(tag);
        }
    }
}
