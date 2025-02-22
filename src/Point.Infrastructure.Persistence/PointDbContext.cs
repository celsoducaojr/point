using Google.Protobuf.Reflection;
using Microsoft.EntityFrameworkCore;
using Point.Core.Application.Contracts;
using Point.Core.Domain.Contracts.Entities;
using Point.Core.Domain.Entities;
using Point.Order.Core.Domain.Entities;

namespace Point.Infrastructure.Persistence
{
    public class PointDbContext(DbContextOptions<PointDbContext> options)
        : DbContext(options), IPointDbContext 
    {
        public DbSet<Tag> Tag => Set<Tag>();
        public DbSet<Unit> Unit => Set<Unit>();
        public DbSet<Category> Category => Set<Category>();
        public DbSet<Supplier> Supplier => Set<Supplier>();
        public DbSet<SupplierTag> SupplierTag => Set<SupplierTag>();
        public DbSet<Item> Item => Set<Item>();
        public DbSet<ItemTag> ItemTag => Set<ItemTag>();
        public DbSet<ItemUnit> ItemUnit => Set<ItemUnit>();
        public DbSet<PurchaseCost> PurchaseCost => Set<PurchaseCost>();
        public DbSet<Stock> Stock => Set<Stock>();
        public DbSet<Customer> Customer => Set<Customer>();
        public DbSet<Sale> Sale => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Tags)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Item>()
                .HasMany(i => i.Units)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemUnit>()
                .HasOne(p => p.PurchaseCost)  // ItemUnit has one PurchaseCost
                .WithOne(r => r.ItemUnit)      // PurchaseCost belongs to a single ItemUnit
                .HasForeignKey<PurchaseCost>(r => r.Id) // FK = PK
                .IsRequired();  // Enforce relationship
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
