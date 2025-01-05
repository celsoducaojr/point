using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Point.Order.Core.Application.Contracts
{
    public interface IOrderContext
    {
        DbSet<Activity> Activities { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
