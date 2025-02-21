using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Order;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.API.Controllers
{
    public class SuppliersController(
        IMediator mediator, 
        IPointDbContext pointDbContext,
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _dbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateSupplierRequest request)
        {
            return await _mediator.Send(request);
        }

        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        {
            var request = new UpdateSupplierRequest(
                id, dto.Name, dto.Remarks, dto.Tags);

            return await _mediator.Send(request);
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var item = await GetSuppliersAsync(id);

            if (!item.Any())
            {
                throw new NotFoundException("Supplier not found.");
            }
            else
            {
                return Results.Ok(item.FirstOrDefault());
            }

        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok((await GetSuppliersAsync()).Distinct().ToList());
        }

        #region Common Query

        private async Task<IEnumerable<GetSupplierResponseDto>> GetSuppliersAsync(int? id = null)
        {
            var query = @"SELECT s.Id, s.Name, s.Remarks,
                        st.Id AS SupplierTagId, st.Id, t.Name
                        FROM Supplier s
                        LEFT JOIN SupplierTag st ON s.Id = st.SupplierId
                        LEFT JOIN Tag t ON st.TagId = t.Id";

            if (id.HasValue)
            {
                query += " WHERE i.Id = @Id";
            }

            var supplierDictionary = new Dictionary<int, GetSupplierResponseDto>();

            var supplier = await _dbConnection.QueryAsync<Supplier, Tag, GetSupplierResponseDto>(
                query,
                (supplier, supplierTag) =>
                {
                    if (!supplierDictionary.TryGetValue(supplier.Id, out var supplierEntry))
                    {
                        supplierEntry = new GetSupplierResponseDto
                        {
                            Id = supplier.Id,
                            Name = supplier.Name,
                            Remarks = supplier.Remarks,
                            Tags = supplierTag?.Id != 0 ? [] : null
                        };
                        supplierDictionary[supplier.Id] = supplierEntry;
                    }

                    if (supplierTag?.Id != 0)
                    {
                        supplierEntry.Tags.Add(supplierTag.Name);
                    }

                    return supplierEntry;
                },
                id.HasValue ? new { Id = id.Value } : null,
                splitOn: "SupplierTagId"
            );

            return supplier;
        }

        #endregion
    }
}
