using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Orders;

namespace Point.API.Controllers.Orders
{
    [Route("api/v{version:apiversion}/customers")]
    public class CustomerController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateCustomerRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateNameDto updateNameDto)
        {
            await _mediator.Send(new UpdateCustomerRequest(id, updateNameDto.Name));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.Customers.FindAsync(id)
                ?? throw new NotFoundException("Customer not found.");

            return Ok(unit);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _pointDbContext.Customers.OrderBy(customer => customer.Name).ToListAsync());
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var unit = await _pointDbContext.Customers
                .Where(customer => EF.Functions.Like(customer.Name, $"%{name}%"))
                .OrderBy(customer => customer.Name)
                .ToListAsync();

            return Ok(unit);
        }
    }
}
