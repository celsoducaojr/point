using System.Data;
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
    [Route("api/v{version:apiversion}/item-units")]
    public class ItemUnitController(
        IMediator mediator, 
        IPointDbContext pointDbContext,
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _pointDbConnection = pointDbConnection.Connection;

        //[HttpPost]
        //public async Task<IActionResult> Add([FromBody] CreateItemUnitRequest request)
        //{
        //    var id = await _mediator.Send(request);

        //    return CreatedAtAction(nameof(GetById), new { id }, new { id });
        //}

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

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var query = $@"
        //        SELECT
        //        i.Id, i.Name, i.Description,
        //        c.Id, c.Name,
        //        it.Id, t.Name,
        //        iu.Id, iu.UnitId, iu.ItemCode, iu.RetailPrice, iu.WholesalePrice, iu.PriceCode, iu.Remarks,
        //        u.Id, u.Name
        //        FROM Items i
        //        {_joinQueryExpression}
        //        WHERE iu.Id = @Id";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("Id", id);

        //    var fields = ApiConstants.ItemUnitFields;

        //    var itemUnit = (await LookupAsync(query, parameters, fields)).FirstOrDefault()
        //         ?? throw new NotFoundException("Item Unit not found.");

        //    return Ok(itemUnit);
        //}

        //[HttpGet("code/{code}")]
        //public async Task<IActionResult> GetByCode(string code)
        //{
        //    var query = $@"
        //        SELECT
        //        i.Id, i.Name, i.Description,
        //        c.Id, c.Name,
        //        it.Id, t.Name,
        //        iu.Id, iu.UnitId, iu.ItemCode, iu.RetailPrice, iu.WholesalePrice, iu.PriceCode, iu.Remarks,
        //        u.Id, u.Name
        //        FROM Items i
        //        {_joinQueryExpression}
        //        WHERE iu.ItemCode = @ItemCode";

        //    var parameters = new DynamicParameters();
        //    parameters.Add("ItemCode", code);

        //    var fields = ApiConstants.ItemUnitFields;

        //    var itemUnit = (await LookupAsync(query, parameters, fields)).FirstOrDefault()
        //         ?? throw new NotFoundException("Item Unit not found.");

        //    return Ok(itemUnit);
        //}

        //[HttpGet("search")]
        //public async Task<IActionResult> Search(
        //    [FromQuery] int page = 1,
        //    [FromQuery] int pageSize = 50,
        //    [FromQuery] string? itemCode = null,
        //    [FromQuery] string? name = null,
        //    [FromQuery] int? categoryId = null,
        //    [FromQuery] List<int>? tagIds = null,
        //    [FromQuery] int? unitId = null,
        //    [FromQuery] List<string>? fields = null)
        //{
        //    name = name?.Trim();
        //    tagIds ??= [];
        //    fields ??= [];

        //    if (fields.Except(ApiConstants.ItemUnitFields).ToList().Any())
        //    {
        //        throw new DomainException("Invalid fields requested.");
        //    }

        //    if (page < 1 || pageSize < 1)
        //    {
        //        throw new DomainException("Invalid pagination requested.");
        //    }

        //    var idsQuery = $@"
        //        SELECT DISTINCT i.Id, i.Name 
        //        FROM Items i
        //        {_joinQueryExpression}";

        //    var conditions = new List<string>();
        //    var parameters = new DynamicParameters();

        //    if (!string.IsNullOrWhiteSpace(itemCode))
        //    {
        //        conditions.Add("iu.ItemCode LIKE @ItemCode");
        //        parameters.Add("ItemCode", $"%{itemCode}%");
        //    }
        //    if (!string.IsNullOrWhiteSpace(name))
        //    {
        //        conditions.Add("i.Name LIKE @Name");
        //        parameters.Add("Name", $"%{name}%");
        //    }
        //    if (categoryId.HasValue)
        //    {
        //        conditions.Add("i.CategoryId = @CategoryId");
        //        parameters.Add("CategoryId", categoryId);
        //    }
        //    if (tagIds.Any())
        //    {
        //        conditions.Add("t.Id IN @TagIds");
        //        parameters.Add("TagIds", tagIds);
        //    }
        //    if (unitId.HasValue)
        //    {
        //        conditions.Add("iu.UnitId = @UnitId");
        //        parameters.Add("UnitId", unitId);
        //    }

        //    // Add search criteria
        //    if (conditions.Any())
        //    {
        //        idsQuery += " WHERE " + string.Join(" AND ", conditions);
        //    }
        //    idsQuery += " ORDER BY i.Name";

        //    // Execute Ids query
        //    var ids = await _pointDbConnection.QueryAsync<int>(idsQuery, parameters);

        //    var pageIds = ids
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();

        //    parameters = new DynamicParameters();
        //    parameters.Add("Ids", pageIds);

        //    var pageQuery = $@"
        //        SELECT
        //        i.Id, i.Name, i.Description,
        //        c.Id, c.Name,
        //        it.Id, t.Name,
        //        iu.Id, iu.UnitId, iu.ItemCode, iu.RetailPrice, iu.WholesalePrice, iu.PriceCode, iu.Remarks,
        //        u.Id, u.Name
        //        FROM Items i
        //        {_joinQueryExpression}
        //        WHERE i.Id in @Ids
        //        ORDER By i.Name";

        //    // Execute page query
        //    var itemUnits = await LookupAsync(pageQuery, parameters, fields);

        //    return Ok(new
        //    {
        //        data = itemUnits,
        //        totalCount = ids.Count(),
        //        page,
        //        pageSize
        //    });
        //}

        #region Queries

        private const string _joinQueryExpression = @"
            LEFT JOIN Categories c ON i.CategoryId = c.Id
            LEFT JOIN ItemTags it ON it.ItemId = i.Id
            LEFT JOIN Tags t ON t.Id = it.TagId
            INNER JOIN ItemUnits iu ON iu.ItemId = i.Id
            INNER JOIN Units u ON u.Id = iu.UnitId";
       
        //private async Task<IEnumerable<GetItemUnitResponseDto>> LookupAsync(string query, DynamicParameters parameters, List<string> fields)
        //{
        //    var itemDictionary = new Dictionary<int, GetItemUnitResponseDto>();
        //    var itemUnits = await _pointDbConnection.QueryAsync<Item, Category, Tag, ItemUnit, Core.Domain.Entities.Unit, GetItemUnitResponseDto>(
        //        query,
        //        (item, category, itemTag, itemUnit, unit) =>
        //        {
        //            if (!itemDictionary.TryGetValue(item.Id, out var itemUnitEntry))
        //            {
        //                itemUnitEntry = new GetItemUnitResponseDto
        //                {
        //                    Id = item.Id,
        //                    Name = item.Name,
        //                    Description = fields.Contains(ApiConstants.EntityFields.Description, StringComparer.OrdinalIgnoreCase) ? item.Description : null,
        //                    Category = fields.Contains(ApiConstants.EntityFields.Category, StringComparer.OrdinalIgnoreCase) && category?.Id > 0 ? category : null,
        //                    Tags = fields.Contains(ApiConstants.EntityFields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0 ? [] : null,
        //                    Unit = unit,
        //                    ItemCode = itemUnit.ItemCode,
        //                    RetailPrice = itemUnit.RetailPrice,
        //                    WholesalePrice = itemUnit.WholesalePrice,
        //                    PriceCode = itemUnit.PriceCode,
        //                    Remarks = fields.Contains(ApiConstants.EntityFields.Remarks, StringComparer.OrdinalIgnoreCase) ? itemUnit.Remarks : null
        //                };
        //                itemDictionary[item.Id] = itemUnitEntry;
        //            }

        //            if (fields.Contains(ApiConstants.EntityFields.Tags, StringComparer.OrdinalIgnoreCase) && itemTag?.Id > 0)
        //            {
        //                itemUnitEntry.Tags.Add(itemTag);
        //            }

        //            return itemUnitEntry;
        //        },
        //        parameters,
        //        splitOn: "Id"
        //    );

        //    return itemUnits.Distinct().ToList();
        //}

        #endregion
    }
}
