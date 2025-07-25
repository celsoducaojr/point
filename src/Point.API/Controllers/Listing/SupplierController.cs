﻿using System.Data;
using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Point.API.Controllers.Base;
using Point.API.Dtos.Listing;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers.Listing;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;

namespace Point.API.Controllers.Listing
{
    [Route("api/v{version:apiversion}/suppliers")]
    public class SupplierController(
        IMediator mediator, 
        IPointDbConnection pointDbConnection) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IDbConnection _dbConnection = pointDbConnection.Connection;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierRequest request)
        {
            var id = await _mediator.Send(request);

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSupplierDto updateSupplierDto)
        {
            await _mediator.Send(new UpdateSupplierRequest(
                id, updateSupplierDto.Name, updateSupplierDto.Remarks, updateSupplierDto.Tags));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await GetSuppliersAsync(id);

            if (!supplier.Any())
            {
                throw new NotFoundException("Supplier not found.");
            }
            else
            {
                return Ok(supplier.FirstOrDefault());
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
                        FROM Suppliers s
                        LEFT JOIN SupplierTags st ON s.Id = st.SupplierId
                        LEFT JOIN Tags t ON st.TagId = t.Id";

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
                            Tags = supplierTag?.Id > 0 ? [] : null
                        };
                        supplierDictionary[supplier.Id] = supplierEntry;
                    }

                    if (supplierTag?.Id > 0)
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
