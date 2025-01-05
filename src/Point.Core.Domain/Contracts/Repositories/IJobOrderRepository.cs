using Point.Order.Core.Domain.Entities;

namespace Point.Core.Domain.Contracts.Repositories
{
    public interface IJobOrderRepository
    {
        Task<JobOrder> GetById(int id);
        Task<IEnumerable<JobOrder>> GetAll();
    }
}
