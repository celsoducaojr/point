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
using Point.Infrastructure.Persistence;
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
        private readonly IDbConnection _pointDbConnection = pointDbConnection.Connection;

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
        public async Task<IActionResult> GetById(int id, [FromQuery] List<string>? fields)
        {
            var itemUnit = (await LookupAsync(id: id, fields: fields)).FirstOrDefault()
                 ?? throw new NotFoundException("Item not found.");

            return Ok(itemUnit);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] int? categoryId,
            [FromQuery] List<int>? tags,
            [FromQuery] List<string>? fields)
        {
            return Ok(await LookupAsync(name: name, categoryId: categoryId, tags: tags, fields: fields));
        }

        #region Queries

        private async Task<IEnumerable<GetItemResponseDto>> LookupAsync(
            int? id = null,
            string? name = null,
            int? categoryId = null,
            List<int>? tags = null,
            List<string>? fields = null)
        {
            name = name?.Trim();
            tags ??= [];
            fields ??= [];

            if (fields.Except(ApiConstants.Item.Fields).ToList().Any())
            {
                throw new DomainException("Invalid fields requested.");
            }

            var query = @"SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                it.Id, t.Name
                FROM Items i
                LEFT JOIN Categories c ON i.CategoryId = c.Id
                LEFT JOIN ItemTags it ON it.ItemId = i.Id
                LEFT JOIN Tags t ON t.Id = it.TagId";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (id.HasValue)
            {
                conditions.Add("i.Id = @Id");
                parameters.Add("Id", id);
            }
            else
            {
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
            }

            if (conditions.Any())
            {
                query += " WHERE " + string.Join(" AND ", conditions);
            }

            var itemDictionary = new Dictionary<int, GetItemResponseDto>();

            var itemUnits = await _pointDbConnection.QueryAsync<Item, Category, Tag, GetItemResponseDto>(
                query,
                (item, category, itemTag) =>
                {
                    if (!itemDictionary.TryGetValue(item.Id, out var itemEntry))
                    {
                        itemEntry = new GetItemResponseDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = fields.Contains(ApiConstants.Fields.Description, StringComparer.OrdinalIgnoreCase) ? item.Description : null,
                            Category = fields.Contains(ApiConstants.Fields.Category, StringComparer.OrdinalIgnoreCase) && category?.Id > 0 ? category : null,
                            Tags = fields.Contains(ApiConstants.Fields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0 ? [] : null
                        };
                        itemDictionary[item.Id] = itemEntry;
                    }

                    if (fields.Contains(ApiConstants.Fields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0)
                    {
                        itemEntry.Tags.Add(itemTag);
                    }

                    return itemEntry;
                },
                parameters,
                splitOn: "Id"
            );

            return itemUnits.Distinct().ToList();
        }

        #endregion
    }
}
