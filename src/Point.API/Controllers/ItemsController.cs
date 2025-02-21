using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.API.Controllers
{
    public class ItemsController(
        IMediator mediator, 
        IPointDbContext pointDbContext, 
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _dbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateItemRequest request)
        {
            return await _mediator.Send(request);
        }

        //[HttpPut("{id}")]
        //public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        //{
        //    var request = new UpdateSupplierRequest(
        //        id, dto.Name, dto.Remarks, dto.Tags);

        //    return await _mediator.Send(request);
        //}

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var query = @"SELECT i.Id, i.Name, i.Description, 
                        i.CategoryId, c.Id, c.Name,
                        it.Id AS ItemTagId, it.Id, t.Name
                        FROM Item i
                        LEFT JOIN Category c ON i.CategoryId = c.Id
                        LEFT JOIN ItemTag it ON i.Id = it.ItemId
                        LEFT JOIN Tag t ON it.TagId = t.Id
                        WHERE i.Id = @Id";

            var itemDictionary = new Dictionary<int, GetItemResponseDto>();

            var item = await _dbConnection.QueryAsync<Item, Category, GetItemTagResponseDto, GetItemResponseDto>(
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
                            Category = category?.Id != 0 ? category : null,
                            Tags = itemTag?.Id != 0 ? [] : null
                        };
                        itemDictionary[item.Id] = itemEntry;
                    }

                    if (itemTag?.Id != 0)
                    {
                        itemEntry.Tags.Add(new GetItemTagResponseDto 
                        { 
                            Id = itemTag.Id, 
                            Name = itemTag.Name 
                        });
                    }

                    return itemEntry;
                },
                new { Id = id },
                splitOn: "CategoryId, ItemTagId"
            );

            return Results.Ok(item.FirstOrDefault());
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _pointDbContext.Item
                .Include(i => i.Tags)
                .ToListAsync());
        }
    }
}
