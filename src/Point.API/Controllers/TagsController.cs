using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Dtos;
using Point.API.Controllers.Models;
using Point.Core.Application.Handlers;
using Point.Core.Application.Handlers.Order;

namespace Point.API.Controllers
{
    public class TagsController(IMediator mediator) : BaseController
    {
        private readonly IMediator _mediator = mediator;

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

    }
}
