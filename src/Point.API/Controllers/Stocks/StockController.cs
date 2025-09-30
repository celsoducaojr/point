using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Constants;
using Point.API.Controllers.Base;
using Point.API.Dtos.Listing;
using Point.API.Dtos.Stocks;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Stocks;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Entities.Stocks;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.API.Controllers.Stocks
{
    [Route("api/v{version:apiversion}/stocks")]
    public class StockController(
        IMediator mediator, 
        IPointDbContext pointDbContext, 
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _pointDbConnection = pointDbConnection.Connection;

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockItemDto updateStockItemDto)
        {
            await _mediator.Send(new UpdateStockItemRequest(id, updateStockItemDto.Type, updateStockItemDto.Quantity, updateStockItemDto.Remarks));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var unit = await _pointDbContext.StockItems.FindAsync(id)
                ?? throw new NotFoundException("Stock not found.");

            return Ok(unit);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 25,
            [FromQuery] string? name = null)
        {
            name = name?.Trim();

            if (page < 1 || pageSize < 1)
            {
                throw new DomainException("Invalid pagination requested.");
            }

            var idsQuery = $@"
                SELECT si.Id, si.ItemUnitId
                FROM StockItems si
                {_joinQueryExpression}";

            var conditions = new List<string>();
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(name))
            {
                conditions.Add("i.Name LIKE @Name");
                parameters.Add("Name", $"%{name}%");
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
                si.Id, si.ItemUnitId, si.Quantity,
                iu.Id, iu.ItemId, iu.UnitId,
                i.Id, i.Name,
                c.Id, c.Name,
                u.Id, u.Name
                FROM StockItems si
                {_joinQueryExpression}
                LEFT JOIN Categories c ON i.CategoryId = c.Id
                LEFT JOIN Units u ON iu.UnitId = u.Id
                WHERE si.Id in @Ids
                ORDER By i.Name";

            var historiesQuery = $@"
                SELECT
                si.Id,
                sh.Id, sh.StockItemId, sh.QuantityChanged, sh.QuantityAfterChange, sh.Type, sh.Remarks, sh.Created
                FROM StockItems si
                LEFT JOIN StockHistories sh ON si.Id = sh.StockItemId
                WHERE si.Id in @Ids
                ORDER BY sh.Created DESC";

            // Execute page query
            var stocks = await LookupAsync(pageQuery, historiesQuery, parameters);

            return Ok(new
            {
                data = stocks,
                totalCount = ids.Count(),
                page,
                pageSize
            });
        }

        #region Queries

        private const string _joinQueryExpression = @"
            LEFT JOIN ItemUnits iu ON si.ItemUnitId = iu.Id
            LEFT JOIN Items i ON iu.ItemId = i.Id";

        private async Task<IEnumerable<SearchStockResponseDto>> LookupAsync(string query, string historiesQuery, DynamicParameters parameters)
        {
            var stockDictionary = new Dictionary<int, SearchStockResponseDto>();
            var stocks = await _pointDbConnection.QueryAsync<Stock, ItemUnit, Item, Category, Core.Domain.Entities.Unit, SearchStockResponseDto>(
                query,
                (stock, itemUnit, item, category, unit) =>
                {
                    if (!stockDictionary.TryGetValue(stock.Id, out var stockEntry))
                    {
                        stockEntry = new SearchStockResponseDto
                        {
                           ItemUnitId = stock.ItemUnitId,
                           ItemName = item.Name,
                           CategoryName = category.Name,
                           ItemUnitName = unit.Name,
                           Quantity = stock.Quantity,
                           Histories = []
                        };
                        stockDictionary[stock.Id] = stockEntry;
                    }

                    return stockEntry;
                },
                parameters,
                splitOn: "Id"
            );

            await _pointDbConnection.QueryAsync<StockItem, StockHistory, StockHistory>(
               historiesQuery,
               (stock, history) =>
               {
                   var stockEntry = stockDictionary[stock.Id];
                   stockEntry.Histories.Add(history);

                   return history;
               },
               parameters,
               splitOn: "Id"
           );

            return stocks.Distinct().ToList();
        }

        #endregion
    }
}
