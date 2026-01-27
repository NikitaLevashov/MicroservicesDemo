
// NotificationService.Infrastructure/Persistence/NotificationDbContext.cs
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Notifications;
using MassTransit;

namespace NotificationService.Infrastructure.Persistence;

public sealed class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        var e = modelBuilder.Entity<Notification>();
        e.ToTable("Notifications");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).ValueGeneratedOnAdd();
        e.Property(x => x.Type).IsRequired().HasMaxLength(50);
        e.Property(x => x.Recipient).IsRequired().HasMaxLength(256);
        e.Property(x => x.Message).IsRequired().HasMaxLength(2000);
        e.Property(x => x.Status).IsRequired().HasMaxLength(20);
        e.Property(x => x.CreatedAtUtc).IsRequired();
        e.Property(x => x.SentAtUtc);
    }
}
