using Microsoft.EntityFrameworkCore;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Orders;
using Point.Core.Domain.Entities.Stocks;

namespace Point.Core.Application.Contracts
{
    public interface IPointDbContext
    {
        // Listing
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

        // Orders
        DbSet<Order> Orders { get; }
        DbSet<OrderItem> OrderItems { get; }
        DbSet<Payment> Payments { get; }
        DbSet<Refund> Refunds { get; }
        DbSet<Customer> Customers { get; }

        // Stocks
        DbSet<StockItem> StockItems { get; }
        DbSet<StockHistory> StockHistories { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
