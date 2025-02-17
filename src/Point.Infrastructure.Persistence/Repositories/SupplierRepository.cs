using Dapper;
using Point.Core.Application.Exceptions;
using Point.Core.Domain.Contracts.Repositories;
using Point.Core.Domain.Entities;
using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.Infrastructure.Persistence.Repositories
{
    public class SupplierRepository(IPointDbConnection pointDbConnection) : ISupplierRepository
    {
        private readonly IPointDbConnection _pointDbConnection = pointDbConnection;

        public async Task<Supplier> GetById(int id)
        {
            var supplier = await _pointDbConnection.Connection
                .QuerySingleOrDefaultAsync<Supplier>("SELECT * FROM Supplier WHERE Id = @Id", new { Id = id });

            return supplier ?? throw new NotFoundException("Supplier not found.");
        }

        public async Task<IEnumerable<Supplier>> GetAll()
        {
            return await _pointDbConnection.Connection.QueryAsync<Supplier>("SELECT * FROM Supplier");
        }
    }
}
