using Point.Order.Core.Domain.Entities;

namespace Point.Order.Core.Domain.Contracts.Repositories
{
    public interface IActivityRepository
    {
        Task<JobOrder> GetById(int id);
        Task<IEnumerable<JobOrder>> GetAll();
    }
}
