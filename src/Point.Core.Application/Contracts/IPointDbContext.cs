using Microsoft.EntityFrameworkCore;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Application.Contracts
{
    public interface IPointDbContext
    {
        DbSet<JobOrder> JobOrders { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
