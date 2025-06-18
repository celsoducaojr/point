using System.Data;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Constants;
using Point.API.Controllers.Base;
using Point.API.Dtos.Listing;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Application.Handlers.Listing;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;

namespace Point.API.Controllers.Listing
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
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemDto updateItemDto)
        {
            await _mediator.Send(new UpdateItemRequest(
                id, updateItemDto.Name, updateItemDto.Description, updateItemDto.CategoryId, updateItemDto.Tags));

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
            var query = $@"SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                it.Id, t.Name
                FROM Items i
                {_joinQueryExpression}
                WHERE i.Id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var fields = ApiConstants.ItemFields;

            var item = (await LookupAsync(query, parameters, fields)).FirstOrDefault()
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

            var idsQuery = $@"
                SELECT DISTINCT i.Id, i.Name 
                FROM Items i
                {_joinQueryExpression}";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

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

            // Add search criteria
            if (conditions.Any())
            {
                idsQuery += " WHERE " + string.Join(" AND ", conditions);
            }
            idsQuery += " ORDER BY i.Name";

            // Execute Ids query
            var ids = await _pointDbConnection.QueryAsync<int>(idsQuery, parameters);

            var pageIds = ids
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            parameters = new DynamicParameters();
            parameters.Add("Ids", pageIds);

            var pageQuery = $@"
                SELECT
                i.Id, i.Name, i.Description,
                c.Id, c.Name,
                t.Id, t.Name
                FROM Items i
                {_joinQueryExpression}
                WHERE i.Id in @Ids
                ORDER By i.Name";

            // Execute page query
            var items = await LookupAsync(pageQuery, parameters, fields);

            return Ok(new {
                data = items,
                totalCount = ids.Count(),
                page,
                pageSize
            });
        }

        #region Queries

        private const string _joinQueryExpression = @"
            LEFT JOIN Categories c ON c.Id = i.CategoryId
            LEFT JOIN ItemTags it ON it.ItemId = i.Id
            LEFT JOIN Tags t ON t.Id = it.TagId";

        private async Task<IEnumerable<GetItemResponseDto>> LookupAsync(string query, DynamicParameters parameters, List<string> fields)
        {
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

            if (fields.Contains(ApiConstants.EntityFields.Units, StringComparer.OrdinalIgnoreCase))
            {
                var unitQuery = $@"
                SELECT
                iu.Id, iu.ItemId, iu.ItemCode, iu.CostPriceCode,
                u.Id, u.Name,
                p.Id, p.Amount, p.PriceTypeId,
                pt.Id, pt.Name, pt.DisplayIndex
                FROM ItemUnits iu
                LEFT JOIN Units u ON u.Id = iu.UnitId
                LEFT JOIN Prices p ON p.ItemUnitId = iu.Id
                LEFT JOIN PriceTypes pt on pt.Id = p.PriceTypeId
                WHERE iu.ItemId in @Ids";

                var itemUnitDictionary = new Dictionary<int, GetItemUnitResponseDto>();
                var itemUnits = await _pointDbConnection.QueryAsync<ItemUnit, Core.Domain.Entities.Unit, Price, PriceType, GetItemUnitResponseDto>(
                    unitQuery,
                    (itemUnit, unit, price, priceType) =>
                    {
                        if (!itemUnitDictionary.TryGetValue(itemUnit.Id, out var itemUnitEntry))
                        {
                            itemUnitEntry = new GetItemUnitResponseDto
                            {
                                Id = itemUnit.Id,
                                Unit = unit?.Id > 0 ? unit : null,
                                ItemCode = itemUnit.ItemCode,
                                CostPriceCode = itemUnit.CostPriceCode,
                                Prices = price?.Id > 0 ? [] : null
                            };
                            itemUnitDictionary[itemUnit.Id] = itemUnitEntry;
                            itemDictionary[itemUnit.ItemId].AddUnit(itemUnitEntry);
                        }

                        if (price?.Id > 0)
                        {
                            itemUnitEntry.Prices.Add(new GetPriceResponseDto
                            {
                                PriceType = new GetPriceTypeResponseDto
                                {
                                    Id = priceType.Id,
                                    Name = priceType.Name,
                                    DisplayIndex = priceType.DisplayIndex
                                },
                                Amount = price.Amount
                            });
                        }

                        return itemUnitEntry;
                    },
                    parameters,
                    splitOn: "Id"
                );
            }

            return items.Distinct().ToList();
        }

        #endregion
    }
}
