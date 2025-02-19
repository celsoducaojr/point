﻿using Dapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point.API.Controllers.Dtos;
using Point.Core.Application.Contracts;
using Point.Core.Application.Exceptions;
using Point.Core.Application.Handlers;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;


namespace Point.API.Controllers
{
    public class TagsController(IMediator mediator, IPointDbContext pointDbContext) : BaseController
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPointDbContext _pointDbContext = pointDbContext;

        [HttpPost]
        public async Task<IResult> Add([FromBody] CreateTagRequest createTagRequest)
        {
            return await _mediator.Send(createTagRequest);
        }


        [HttpPut("{id}")]
        public async Task<IResult> Update([FromRoute] int id, [FromBody] UpdateTagDto dto)
        {
            return await _mediator.Send(new UpdateTagRequest(
                id, dto.Name));
        }

        [HttpGet("{id}")]
        public async Task<IResult> GetById(int id)
        {
            var supplier = (await _pointDbContext.Tag
                .FirstOrDefaultAsync(t => t.Id == id))
                ?? throw new NotFoundException("Tag not found.");

            return Results.Ok(supplier);
        }

        [HttpGet]
        public async Task<IResult> GetAll()
        {
            return Results.Ok(await _pointDbContext.Tag
                .ToListAsync());
        }
    }
}
