﻿using System.Data;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Constants;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;

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
        public async Task<IActionResult> GetById(int id)
        {
            var query = @"SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                it.Id, t.Name
                FROM Items i
                LEFT JOIN Categories c ON i.CategoryId = c.Id
                LEFT JOIN ItemTags it ON it.ItemId = i.Id
                LEFT JOIN Tags t ON t.Id = it.TagId
                WHERE i.Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var fields = ApiConstants.ItemFields;

            var item = (await QueryItems(query, parameters, fields)).FirstOrDefault()
                 ?? throw new NotFoundException("Item not found.");

            return Ok(item);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? name = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] List<int>? tagIds = null,
            [FromQuery] List<string>? fields = null)
        {
            var result = await LookupAsync(page: page, pageSize: pageSize, name: name, categoryId: categoryId, tagIds: tagIds, fields: fields);

            return Ok(new {
                data = result.Items,
                totalCount = result.TotalCount,
                page,
                pageSize
            });
        }

        #region Queries

        private async Task<(IEnumerable<GetItemResponseDto> Items, int TotalCount)> LookupAsync(
            int? id = null,
            int page = 1,
            int pageSize = 25,
            string? name = null,
            int? categoryId = null,
            List<int>? tagIds = null,
            List<string>? fields = null)
        {
            name = name?.Trim();
            tagIds ??= [];
            fields ??= [];

            if (fields.Except(ApiConstants.ItemFields).ToList().Any())
            {
                throw new DomainException("Invalid fields requested.");
            }

            if (page < 1 || pageSize < 1)
            {
                throw new DomainException("Invalid pagination requested.");
            }

            var countQuery = @"SELECT COUNT(DISTINCT i.Id) FROM Items i";

            var pageQuery = @"SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                it.Id, t.Name
                FROM (SELECT Id FROM Items ORDER BY Name LIMIT @PageSize OFFSET @Offset) AS limitedIds
                JOIN Items i ON i.Id = limitedIds.Id";

            var filter = @"
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
                parameters.Add("Offset", (page - 1) * pageSize);
                parameters.Add("PageSize", pageSize);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    conditions.Add("i.Name LIKE @Name");
                    parameters.Add("Name", $"%{name}%");
                }

                if (categoryId.HasValue)
                {
                    conditions.Add("i.CategoryId = @CategoryId");
                    parameters.Add("CategoryId", categoryId);
                }

                if (tagIds.Any())
                {
                    conditions.Add("t.Id IN @TagIds");
                    parameters.Add("TagIds", tagIds);
                }
            }

            if (conditions.Any())
            {
                filter += " WHERE " + string.Join(" AND ", conditions);
            }

            countQuery += $" {filter};";

            pageQuery += $" {filter};";

            var totalCount = await _pointDbConnection.QuerySingleAsync<int>(countQuery, parameters);

            var items = await QueryItems(pageQuery, parameters, fields);

            return (items, totalCount);
        }

        private async Task<IEnumerable<GetItemResponseDto>> QueryItems(string query, DynamicParameters parameters, List<string> fields)
        {
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
                            Description = fields.Contains(ApiConstants.EntityFields.Description, StringComparer.OrdinalIgnoreCase) ? item.Description : null,
                            Category = fields.Contains(ApiConstants.EntityFields.Category, StringComparer.OrdinalIgnoreCase) && category?.Id > 0 ? category : null,
                            Tags = fields.Contains(ApiConstants.EntityFields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0 ? [] : null
                        };
                        itemDictionary[item.Id] = itemEntry;
                    }

                    if (fields.Contains(ApiConstants.EntityFields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0)
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
