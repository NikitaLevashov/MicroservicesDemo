
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProductService.Infrastructure
{
    public class ProductDbContext : DbContext
    {
        private readonly StreamWriter _logStream;

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
            var logFilePath = Path.Combine(AppContext.BaseDirectory, "mylog.txt");
            _logStream = new StreamWriter(logFilePath, append: true)
            {
                AutoFlush = true
            };
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });

            Console.WriteLine("Model configuration completed.");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .LogTo(_logStream.WriteLine, new[] { RelationalEventId.CommandExecuted })
                .EnableSensitiveDataLogging();
        }

        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }
    }
}
