using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
using Point.API.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Order;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.API.Controllers
{
    [Route("api/v{version:apiversion}/suppliers")]
    public class SupplierController(
        IMediator mediator, 
        IPointDbContext pointDbContext,
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;
        private readonly IDbConnection _dbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSupplierRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto dto)
        {
            await _mediator.Send(new UpdateSupplierRequest(
                id, dto.Name, dto.Remarks, dto.Tags));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await GetSuppliersAsync(id);

            if (!item.Any())
            {
                throw new NotFoundException("Supplier not found.");
            }
            else
            {
                return Ok(item.FirstOrDefault());
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await GetSuppliersAsync()).Distinct().ToList());
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
                query += " WHERE s.Id = @Id";
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
