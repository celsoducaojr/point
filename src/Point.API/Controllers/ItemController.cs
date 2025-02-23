using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var item = await GetItemsAsync(id);

            if (!item.Any())
            {
                throw new NotFoundException("Item not found.");
            }
            else
            {
                return Ok(item.FirstOrDefault());
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await GetItemsAsync()).Distinct().ToList());
        }

        #region Common Query

        private async Task<IEnumerable<GetItemResponseDto>> GetItemsAsync(int? id = null)
        {
            var query = @"SELECT i.Id, i.Name, i.Description, 
                        i.CategoryId, c.Id, c.Name,
                        it.Id AS ItemTagId, it.Id, t.Name
                        FROM Items i
                        LEFT JOIN Categories c ON i.CategoryId = c.Id
                        LEFT JOIN ItemTags it ON i.Id = it.ItemId
                        LEFT JOIN Tags t ON it.TagId = t.Id";

            if (id.HasValue)
            {
                query += " WHERE i.Id = @Id";
            }

            var itemDictionary = new Dictionary<int, GetItemResponseDto>();

            var item = await _dbConnection.QueryAsync<Item, Category, Tag, GetItemResponseDto>(
                query,
                (item, category, itemTag) =>
                {
                    if (!itemDictionary.TryGetValue(item.Id, out var itemEntry))
                    {
                        itemEntry = new GetItemResponseDto
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description,
                            Category = category?.Id != 0 ? category.Name : null,
                            Tags = itemTag?.Id != 0 ? [] : null
                        };
                        itemDictionary[item.Id] = itemEntry;
                    }

                    if (itemTag?.Id != 0)
                    {
                        itemEntry.Tags.Add(itemTag.Name);
                    }

                    return itemEntry;
                },
                id.HasValue ? new { Id = id.Value } : null,
                splitOn: "CategoryId, ItemTagId"
            );

            return item;
        }

        #endregion

    }
}
