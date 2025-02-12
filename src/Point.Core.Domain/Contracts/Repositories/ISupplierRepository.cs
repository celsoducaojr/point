using Point.Core.Domain.Entities;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Domain.Contracts.Repositories
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetById(int id);
        Task<IEnumerable<Supplier>> GetAll();
    }
}
