using Microsoft.EntityFrameworkCore;
using OrderProcessing.IdempotentApi.Domain.Entities;

namespace OrderProcessing.IdempotentApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(o => o.ProductName).IsRequired().HasMaxLength(200);
            entity.Property(o => o.UnitPrice).HasPrecision(18, 2);
            entity.Property(o => o.Status).HasConversion<string>();
        });

        modelBuilder.Entity<IdempotencyRecord>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.IdempotencyKey).IsUnique();
            entity.Property(r => r.IdempotencyKey).IsRequired().HasMaxLength(500);
            entity.Property(r => r.ResponsePayload).IsRequired();
        });
    }
}
