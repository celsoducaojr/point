using Microsoft.EntityFrameworkCore;
using Point.Core.Domain.Entities;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Application.Contracts
{
    public interface IPointDbContext
    {
        DbSet<Tag> Tags { get; }
        DbSet<Unit> Units { get; }
        DbSet<Category> Categories { get; }
        DbSet<Supplier> Suppliers { get; }
        DbSet<SupplierTag> SupplierTags { get; }
        DbSet<Item> Items { get; }
        DbSet<ItemTag> ItemTags { get; }
        DbSet<ItemUnit> ItemUnits { get; }
        DbSet<CostReference> CostReferences { get; }
        DbSet<DiscountVariation> DiscountVariations { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
