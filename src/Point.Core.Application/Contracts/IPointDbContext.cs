using Microsoft.EntityFrameworkCore;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Orders;

namespace Point.Core.Application.Contracts
{
    public interface IPointDbContext
    {
        DbSet<Tag> Tags { get; }
        DbSet<Unit> Units { get; }
        DbSet<PriceType> PriceTypes { get; }
        DbSet<Category> Categories { get; }
        DbSet<Supplier> Suppliers { get; }
        DbSet<SupplierTag> SupplierTags { get; }
        DbSet<Item> Items { get; }
        DbSet<ItemTag> ItemTags { get; }
        DbSet<ItemUnit> ItemUnits { get; }
        DbSet<Price> Prices { get; }
        DbSet<CostReference> CostReferences { get; }
        DbSet<DiscountVariation> DiscountVariations { get; }

        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Refund> Refunds { get; }
        DbSet<Customer> Customers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
