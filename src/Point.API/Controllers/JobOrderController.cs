using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Models;
using Point.Core.Application.Handlers.Order;
using Point.Core.Domain.Contracts.Repositories;

namespace Point.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class JobOrderController(IMediator mediator, IJobOrderRepository jobOrderRepository) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IJobOrderRepository _jobOrderRepository = jobOrderRepository;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateJobOrderRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            return Results.Ok(await _jobOrderRepository.GetById(id));
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _jobOrderRepository.GetAll());
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateJobOrderModel model)
        {
            var request = new UpdateJobOrderRequest(
                id);

            return await _mediator.Send(request);
        }
    }
}
