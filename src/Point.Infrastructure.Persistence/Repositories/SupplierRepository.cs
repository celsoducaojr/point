using Dapper;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Contracts.Repositories;
using Point.Core.Domain.Entities;
using Point.Order.Core.Domain.Entities;
using System.Data;

namespace Point.Infrastructure.Persistence.Repositories
{
    public class SupplierRepository(IDbConnection dbConnection) : ISupplierRepository
    {
        private readonly IDbConnection _dbConnection = dbConnection;

        public async Task<Supplier> GetById(int id)
        {
            var supplier = await _dbConnection.QuerySingleOrDefaultAsync<Supplier>(
                "SELECT * FROM Supplier WHERE Id = @Id", new { Id = id });

            return supplier ?? throw new NotFoundException("Supplier not found.");
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await _dbConnection.QueryAsync<Supplier>("SELECT * FROM Supplier");
        }
    }
}
