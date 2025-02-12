﻿using Dapper;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Contracts.Repositories;
using Point.Order.Core.Domain.Entities;
using System.Data;

namespace Point.Infrastructure.Persistence.Repositories
{
    public class JobOrderRepository(IDbConnection dbConnection) : IJobOrderRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<Sale> GetById(int id)
        {
            var jobOrder = await _dbConnection.QuerySingleOrDefaultAsync<Sale>(
                "SELECT * FROM JobOrders WHERE Id = @Id", new { Id = id });

            return jobOrder ?? throw new NotFoundException("Job Order not found.");
        }

        public async Task<IEnumerable<Sale>> GetAll()
        {
            return await _dbConnection.QueryAsync<Sale>("SELECT * FROM JobOrders");
        }
    }
}
