using Point.Infrastructure.Persistence.Contracts;
using System.Data;

namespace Point.Infrastructure.Persistence
{
    public class PointDbConnection(IDbConnection dbConnection) : IPointDbConnection
    {
        private readonly IDbConnection _dbConnection = dbConnection;
        public IDbConnection Connection { get { return _dbConnection; } }
    }
}
