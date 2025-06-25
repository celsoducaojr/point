using System.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
using Point.API.Dtos.Listing;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Listing;

namespace Point.API.Controllers.Listing
{
    [Route("api/v{version:apiversion}/item-units")]
    public class ItemUnitController(
        IMediator mediator, 
        IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemUnitRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemUnitDto updateItemUnitDto)
        {
            await _mediator.Send(new UpdateItemUnitRequest(
                id, updateItemUnitDto.ItemId, updateItemUnitDto.UnitId, updateItemUnitDto.ItemCode, updateItemUnitDto.PriceCode, updateItemUnitDto.Prices));

            return NoContent();
        }

        [HttpPatch("")]
        public async Task<IActionResult> Patch([FromBody]PatchItemUnitsRequest patchItemUnitsRequest)
        {
            await _mediator.Send(patchItemUnitsRequest);

            return NoContent();
        }

        [HttpPut("{id}/cost")]
        public async Task<IActionResult> UpdateCost([FromRoute] int id, [FromBody] UpdateCostReferenceDto dto)
        {
            await _mediator.Send(new UpdateCostReferenceRequest(
                id, dto.InitialAmount, dto.FinalAmount, 
                dto.Variations?.Select(x => new UpdateDiscountVariationRequest(x.Amount, x.Percentage, x.Remarks)).ToList()));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.ItemUnits.FindAsync(id)
               ?? throw new NotFoundException("Item-unit not found.");

            return Ok(unit);
        }
    }
}
