
using Microsoft.EntityFrameworkCore;
using PurchaseService.Domain;
using PurchaseService.Domain;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace PurchaseService.Infrastructure
{
    public class PurchaseDbContext : DbContext
    {
        public PurchaseDbContext(DbContextOptions<PurchaseDbContext> options) : base(options) { }

        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchases");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.HasMany(e => e.Items).WithOne().HasForeignKey(i => i.PurchaseId);
            });

            modelBuilder.Entity<PurchaseItem>(entity =>
            {
                entity.ToTable("PurchaseItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });
        }
    }
}
