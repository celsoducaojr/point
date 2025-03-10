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
using static Mysqlx.Expect.Open.Types;

namespace Point.API.Controllers
{
    [Route("api/v{version:apiversion}/item-units")]
    public class ItemUnitController(
        IMediator mediator, 
        IPointDbContext pointDbContext,
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _pointDbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateItemUnitRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemUnitDto dto)
        {
            await _mediator.Send(new UpdateItemUnitRequest(
                id, dto.ItemId, dto.UnitId, dto.ItemCode, dto.RetailPrice, dto.WholeSalePrice, dto.PriceCode, dto.Remarks));

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
            var itemUnit = await _pointDbContext.ItemUnits
                .FirstOrDefaultAsync(i => i.Id == id)
                ?? throw new NotFoundException("Item Unit not found.");

            return Ok(itemUnit);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] List<int>? tags,
            [FromQuery] List<string>? fields)
        {
            name = name?.Trim();
            tags ??= [];
            fields ??= [];

            if (fields.Except(ApiConstants.Items.QueryFields).ToList().Any())
            {
                throw new DomainException("Invalid fields requested.");
            }

            var query = @"SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                it.Id, t.Name
                FROM Items i
                LEFT JOIN Categories c ON i.CategoryId = c.Id
                LEFT JOIN ItemTags it ON i.Id = it.ItemId
                LEFT JOIN Tags t ON it.TagId = t.Id";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(name))
            {
                conditions.Add("i.Name LIKE @Name");
                parameters.Add("Name", $"%{name}%");
            }

            if (categoryId.HasValue)
            {
                conditions.Add("i.CategoryId = @CategoryId");
                parameters.Add("CategoryId", categoryId);
            }

            if (tags.Any())
            {
                conditions.Add("t.Id IN @TagIds");
                parameters.Add("TagIds", tags);
            }

            if (conditions.Any())
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            var itemDictionary = new Dictionary<int, GetItemResponseDto>();

            var items = await _pointDbConnection.QueryAsync<Item, Category, Tag, GetItemResponseDto>(
                query,
                (item, category, itemTag) =>
                {
                    if (!itemDictionary.TryGetValue(item.Id, out var itemEntry))
                    {
                        itemEntry = new GetItemResponseDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = fields.Contains(ApiConstants.Items.Fields.Description, StringComparer.OrdinalIgnoreCase) ? item.Description : null,
                            Category = fields.Contains(ApiConstants.Items.Fields.Category, StringComparer.OrdinalIgnoreCase) && category?.Id > 0 ? category.Name : null,
                            Tags = fields.Contains(ApiConstants.Items.Fields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id != 0 ? [] : null
                        };
                        itemDictionary[item.Id] = itemEntry;
                    }

                    if (fields.Contains(ApiConstants.Items.Fields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id != 0)
                    {
                        itemEntry.Tags.Add(itemTag.Name);
                    }

                    return itemEntry;
                },
                parameters,
                splitOn: "Id"
            );

            return Ok(items.Distinct().ToList());
        }
    }
}
