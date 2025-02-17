using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Models;
using Point.Core.Application.Handlers.Order;
using Point.Core.Domain.Contracts.Repositories;

namespace Point.API.Controllers
{
    public class SupplierController(IMediator mediator, ISupplierRepository supplierRepository) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly ISupplierRepository _supplierRepository = supplierRepository;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateSupplierRequest createSupplierRequest)
        {
            return await _mediator.Send(createSupplierRequest);
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
