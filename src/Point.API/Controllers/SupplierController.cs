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
    public class SupplierController(IMediator mediator, ISupplierRepository supplierRepository) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ISupplierRepository _supplierRepository = supplierRepository;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateSupplierRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        {
            var request = new UpdateSupplierRequest(
                id, dto.Name, dto.Remarks);

            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            return Results.Ok(await _supplierRepository.GetById(id));
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _supplierRepository.GetAll());
        }

        
    }
}
