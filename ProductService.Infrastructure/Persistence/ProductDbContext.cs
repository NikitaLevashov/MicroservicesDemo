
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;

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

    public class UserCompanyDto
    {
        public int? UserId { get; set; } // nullable
        public string? Name { get; set; }
        public int CompanyId { get; set; }
        public string? Company { get; set; }
    }

    // Test
    [Table("Country")]
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Region { get; set; }
        public string? ISO { get; set; } = null!;
        public List<Company> Companies { get; set; } = new();
    }

    public class Company
    {
        public int Id { get; set; }
        public string? Name { get; set; } // название компании

        public Country Country { get; set; } = new();

        public int CountryId { get; set; } = new();

        public List<User> Users { get; set; } = new();
    }

    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }

        public int? CompanyInfoKey { get; set; }      // внешний ключ
        public Company? Company { get; set; }    // навигационное свойство
    }
}
