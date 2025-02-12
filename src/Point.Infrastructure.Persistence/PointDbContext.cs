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
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Item> Item { get; set; }
        
        public DbSet<JobOrder> JobOrder => Set<JobOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
