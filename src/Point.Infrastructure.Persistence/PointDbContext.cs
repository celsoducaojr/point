using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Entities;
using Point.Core.Domain.Entities.Orders;

namespace Point.Infrastructure.Persistence
{
    public class PointDbContext(DbContextOptions<PointDbContext> options)
        : DbContext(options), IPointDbContext 
    {
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Unit> Units => Set<Unit>();
        public DbSet<PriceType> PriceTypes => Set<PriceType>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<SupplierTag> SupplierTags => Set<SupplierTag>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemTag> ItemTags => Set<ItemTag>();
        public DbSet<ItemUnit> ItemUnits => Set<ItemUnit>();
        public DbSet<Price> Prices => Set<Price>();
        public DbSet<CostReference> CostReferences => Set<CostReference>();
        public DbSet<DiscountVariation> DiscountVariations => Set<DiscountVariation>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Refund> Refunds => Set<Refund>();
        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cascade delete
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Tags)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemUnit>()
                .HasOne(i => i.CostReference)  // ItemUnit has one Cost
                .WithOne(c => c.ItemUnit)      // Cost belongs to a single ItemUnit
                .HasForeignKey<CostReference>(c => c.Id) // FK = PK
                .IsRequired() // Enforce relationship
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Price>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.SubTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Order>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Discount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Total)
                .HasPrecision(18, 2);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entities = base.ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditable
                && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var now = DateTime.Now;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added) ((IAuditable)entity.Entity).Created = now;

                ((IAuditable)entity.Entity).LastModified = now;
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
