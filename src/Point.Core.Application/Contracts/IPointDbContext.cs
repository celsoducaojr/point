﻿using Microsoft.EntityFrameworkCore;
using Point.Core.Domain.Entities;
using Point.Order.Core.Domain.Entities;

namespace Point.Core.Application.Contracts
{
    public interface IPointDbContext
    {
        DbSet<Tag> Tag { get; }
        DbSet<Unit> Unit { get; }
        DbSet<Category> Category { get; }
        DbSet<Supplier> Supplier { get; }
        DbSet<Item> Item { get; }
        DbSet<Stock> Stock { get; }
        DbSet<Customer> Customer { get; }
        DbSet<Sale> Sale { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
