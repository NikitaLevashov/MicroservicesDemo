
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProductService.Domain.Entities;
using ProductService.Domain.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using static ProductService.Domain.Entities.UserTest;

namespace ProductService.Infrastructure.Persistence
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
        public DbSet<Country> Countries { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<UserCompanyDto> UserCompanyDto { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductDbContext).Assembly);


            var priceConverter = new ValueConverter<Price, decimal>(
                   p => p.Value,
                   v => new Price(v));

            var priceComparer = new ValueComparer<Price>(
                (l, r) => l.Value == r.Value,
                v => v.Value.GetHashCode(),
                v => new Price(v.Value));

            modelBuilder.Entity<Product>(cfg =>
            {
                cfg.ToTable("Products");
                cfg.HasKey(p => p.Id);

                cfg.Property(p => p.Id)
                   .HasColumnName("Id");


                cfg.Property(p => p.Name)
                   .HasMaxLength(200)
                   .IsRequired();

                cfg.Property(p => p.StockQuantity)
                   .IsRequired();

                // ВАЖНО — вот правильный маппинг Price
                cfg.Property(p => p.Price)
                   .HasConversion(priceConverter)
                   .HasColumnName("Price")
                   .HasPrecision(18, 2)
                   .IsRequired();
            });

            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Country>().
                HasMany(c => c.Companies)
                .WithOne(c => c.Country)
                .HasForeignKey(x => x.CountryId);

            modelBuilder.Entity<UserCompanyDto>().HasNoKey();
            //modelBuilder.Entity<User>().ToTable("Users", schema: "userstore");



            modelBuilder.Entity<User>().ToTable("Users", schema: "userstore");
            modelBuilder.Entity<User>().
                HasOne(u => u.Company).
                WithMany(c => c.Users).
                HasForeignKey(fk => fk.CompanyInfoKey).
                OnDelete(DeleteBehavior.Restrict);

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
