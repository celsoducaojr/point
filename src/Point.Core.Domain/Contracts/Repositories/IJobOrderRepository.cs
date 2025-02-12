using Point.Order.Core.Domain.Entities;

namespace Point.Core.Domain.Contracts.Repositories
{
    public interface IJobOrderRepository
    {
        Task<Sale> GetById(int id);
        Task<IEnumerable<Sale>> GetAll();
    }
}
