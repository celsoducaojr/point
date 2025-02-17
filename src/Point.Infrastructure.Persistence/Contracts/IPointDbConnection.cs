using System.Data;

namespace Point.Infrastructure.Persistence.Contracts
{
    public interface IPointDbConnection 
    {
        IDbConnection Connection { get; }
    }
}
